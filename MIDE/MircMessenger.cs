using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static MIDE.Program;

namespace MIDE {
#if (!MONO)
    /// <summary>
    ///     Provides a wrapper that communicates with mIRC using SendMessage.
    /// </summary>
    /// <remarks>
    ///     For more information, see http://en.wikichip.org/wiki/mirc/sendmessage.
    /// </remarks>
    public class MircMessenger : IDisposable {
        private static int lastFileName = 0;
        /// <summary>Returns the size, in bytes, of the mapped file used to share data.</summary>
        public int FileSize { get; }

        public const uint WM_MCOMMAND  = 0x04C8;
        public const uint WM_MEVALUATE = 0x04C9;

        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        /// <summary>Holds the delegate passed to SendMessageCallback to prevent it from being garbage collected.</summary>
        private SendMessageDelegate currentDelegate;
        
        /// <summary>Returns the window handle that messages will be sent to.</summary>
        public IntPtr targetWindow { get; }
        /// <summary>Returns the numeric part of the name of the mapped file used to transfer data.</summary>
        public IntPtr fileNumber { get; }

        private MemoryMappedFile file;
        private MemoryMappedViewAccessor view;

        /// <summary>Creates a new <see cref="MircMessenger"/> object to communicate with a certain mIRC instance.</summary>
        /// <param name="targetWindow">The handle of a window in the target mIRC instance.</param>
        public MircMessenger(IntPtr targetWindow) : this(targetWindow, 1024) { }
        /// <summary>Creates a new <see cref="MircMessenger"/> object to communicate with a certain mIRC instance.</summary>
        /// <param name="targetWindow">The handle of a window in the target mIRC instance.</param>
        /// <param name="fileSize">The size, in bytes, of the mapped file and buffer.</param>
        public MircMessenger(IntPtr targetWindow, int fileSize) {
            if (targetWindow == IntPtr.Zero) throw new ArgumentNullException(nameof(targetWindow), nameof(targetWindow) + " cannot be null.");
            if (fileSize <= 0) throw new ArgumentOutOfRangeException(nameof(fileSize), nameof(fileSize) + " must be positive.");

            this.targetWindow = targetWindow;
            this.fileNumber = new IntPtr(Interlocked.Increment(ref lastFileName));
            this.FileSize = fileSize;

            // Create a mapped file.
            this.file = MemoryMappedFile.CreateNew("mIRC" + this.fileNumber, fileSize);
            this.view = this.file.CreateViewAccessor();
        }

        /// <summary>Closes the mapped file used by this <see cref="MircMessenger"/>.</summary>
        public void Dispose() {
            this.file.Dispose();
        }

        private void prepareMappedFile(uint message, string command, byte[] buffer) {
            // Apparently mIRC doesn't speak UTF-8, so we use UTF-16 instead.
            int n = Encoding.Unicode.GetBytes(command, 0, command.Length, buffer, 0);
            if (n >= (message == WM_MEVALUATE ? FileSize - 4 : FileSize - 2))
                throw new ArgumentException("Expression is too long.");

            this.view.WriteArray(0, buffer, 0, n);  // Data
            this.view.Write(n, (short) 0);          // Null terminator

            if (message == WM_MEVALUATE) {
                this.view.Write(this.FileSize - 4, (short) 0);
                this.view.Write(this.FileSize - 4, unchecked((short) 0xDC00));  // This sequence is not valid in UTF-16, so we use it to determine whether mIRC actually returned any data or not.
            }
        }

        private string getResult(uint message, byte[] buffer) {
            if (message == WM_MEVALUATE) {
                // Read the result.
                this.view.ReadArray(0, buffer, 0, this.FileSize);

                // Earlier, we wrote an invalid UTF-16 low surrogate at FileSize - 2. Check whether that's still there (mIRC will erase it).
                // Return null if mIRC didn't touch that tag.
                if (buffer[FileSize - 4] == 0 && buffer[FileSize - 3] == 0 && BitConverter.ToUInt16(buffer, FileSize - 2) == 0xDC00)
                    return null;

                int i;
                for (i = 0; i < FileSize; i += 2) if (buffer[i] == 0 && buffer[i + 1] == 0) break;  // This searches for a null terminator.
                return Encoding.Unicode.GetString(buffer, 0, i);
            }
            return null;
        }

        /// <summary>Sends a command line to mIRC to execute.</summary>
        /// <param name="command">The command line to send.</param>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public void Run(string command) => this.Run(command, 0);
        /// <summary>Sends a command line to mIRC to execute.</summary>
        /// <param name="command">The command line to send.</param>
        /// <param name="eventID">A number identifying an event context in which the command should be run.</param>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public void Run(string command, short eventID) => dispatchMessage(WM_MCOMMAND, command, eventID);

        /// <summary>Sends an expression to mIRC to evaluate and returns the result.</summary>
        /// <param name="expression">The expression to send.</param>
        /// <returns>The result of the evaluation, or null if an error occurred during evaluation.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public string Evaluate(string expression) => this.Evaluate(expression, 0);
        /// <summary>Sends an expression to mIRC to evaluate and returns the result.</summary>
        /// <param name="expression">The expression to send.</param>
        /// <param name="eventID">A number identifying an event context in which the command should be run.</param>
        /// <returns>The result of the evaluation, or null if an error occurred during evaluation.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public string Evaluate(string expression, short eventID) => dispatchMessage(WM_MEVALUATE, expression, eventID);

        private string dispatchMessage(uint message, string command, short eventID) {
            var buffer = new byte[FileSize];

            prepareMappedFile(message, command, buffer);

            // Send the command to mIRC.
            var result = SendMessageW(this.targetWindow, message, new IntPtr(24 | eventID << 16), this.fileNumber);
            if (result != IntPtr.Zero)
                // Uh oh, error response.
                throw new MircException((MircErrorCode) result);

            return getResult(message, buffer);
        }

        /// <summary>Sends a command line to mIRC to execute and asynchronously waits for mIRC to process the command.</summary>
        /// <param name="command">The command line to send.</param>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public Task RunAsync(string command) => RunAsync(command, 0, CancellationToken.None);
        /// <summary>Sends a command line to mIRC to execute and asynchronously waits for mIRC to process the command.</summary>
        /// <param name="command">The command line to send.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the task.</param>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public Task RunAsync(string command, CancellationToken token) => RunAsync(command, 0, token);
        /// <summary>Sends a command line to mIRC to execute and asynchronously waits for mIRC to process the command.</summary>
        /// <param name="command">The command line to send.</param>
        /// <param name="eventID">A number identifying an event context in which the command should be run.</param>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public Task RunAsync(string command, short eventID) => RunAsync(command, eventID, CancellationToken.None);
        /// <summary>Sends a command line to mIRC to execute and asynchronously waits for mIRC to process the command.</summary>
        /// <param name="command">The command line to send.</param>
        /// <param name="eventID">A number identifying an event context in which the command should be run.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the task.</param>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public Task RunAsync(string command, short eventID, CancellationToken token) => queueMessageAsync(WM_MCOMMAND, command, eventID, token);

        /// <summary>Sends an expression to mIRC to evaluate and asynchronously waits for mIRC to process it.</summary>
        /// <param name="expression">The expression to send.</param>
        /// <returns>The result of the evaluation, or null if an error occurred during evaluation.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public Task<string> EvaluateAsync(string expression) => EvaluateAsync(expression, 0, CancellationToken.None);
        /// <summary>Sends an expression to mIRC to evaluate and asynchronously waits for mIRC to process it.</summary>
        /// <param name="expression">The expression to send.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the task.</param>
        /// <returns>The result of the evaluation, or null if an error occurred during evaluation.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public Task<string> EvaluateAsync(string expression, CancellationToken token) => EvaluateAsync(expression, 0, token);
        /// <summary>Sends an expression to mIRC to evaluate and asynchronously waits for mIRC to process it.</summary>
        /// <param name="expression">The expression to send.</param>
        /// <param name="eventID">A number identifying an event context in which the command should be run.</param>
        /// <returns>The result of the evaluation, or null if an error occurred during evaluation.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public Task<string> EvaluateAsync(string expression, short eventID) => EvaluateAsync(expression, eventID, CancellationToken.None);
        /// <summary>Sends an expression to mIRC to evaluate and asynchronously waits for mIRC to process it.</summary>
        /// <param name="expression">The expression to send.</param>
        /// <param name="eventID">A number identifying an event context in which the command should be run.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the task.</param>
        /// <returns>The result of the evaluation, or null if an error occurred during evaluation.</returns>
        /// <exception cref="ArgumentException"><paramref name="expression"/> is too long to fit in the buffer as UTF-16.</exception>
        /// <exception cref="MircException">mIRC returned an error message.</exception>
        public Task<string> EvaluateAsync(string expression, short eventID, CancellationToken token) => queueMessageAsync(WM_MEVALUATE, expression, eventID, token);

        /// <summary>This method uses a <see cref="SemaphoreSlim"/> to queue messages. Only one is processed at a time.</summary>
        /// <remarks>Thanks to canton7 of the freenode IRC network for this trick.</remarks>
        private async Task<string> queueMessageAsync(uint message, string command, short eventID, CancellationToken token) {
            await semaphore.WaitAsync(token);
            token.ThrowIfCancellationRequested();
            try {
                return await dispatchMessageAsync(message, command, eventID, token);
            } finally {
                semaphore.Release();
            }
        }

        /// <summary>Actually sends the data to mIRC.</summary>
        private Task<string> dispatchMessageAsync(uint message, string command, short eventID, CancellationToken token) {
            var taskSource = new TaskCompletionSource<string>();
            var buffer = new byte[FileSize];

            prepareMappedFile(message, command, buffer);

            // Send the command to mIRC.
            currentDelegate = this.commandCallback;
            SendMessageCallbackW(this.targetWindow, message, new IntPtr(24 | eventID << 16), this.fileNumber, currentDelegate, (IntPtr) GCHandle.Alloc(taskSource));
            token.Register(() => this.cancelled(taskSource));

            return taskSource.Task;
        }

        private void commandCallback(IntPtr hWnd, uint uMsg, IntPtr dwData, IntPtr lResult) {
            var handle = (GCHandle) dwData;
            var taskSource = (TaskCompletionSource<string>) handle.Target;
            handle.Free();
            currentDelegate = null;

            if (taskSource.Task.IsCompleted) return;

            if (lResult != IntPtr.Zero) {
                // Uh oh, error response.
                taskSource.SetException(new MircException((MircErrorCode) lResult));
                return;
            }

            if (uMsg == WM_MEVALUATE) {
                byte[] buffer = new byte[FileSize];
                taskSource.TrySetResult(getResult(uMsg, buffer));
            } else {
                taskSource.TrySetResult(null);
            }
        }

        private void cancelled(TaskCompletionSource<string> taskSource) {
            taskSource.TrySetCanceled();
        }
    }

    /// <summary>
    /// The exception that is thrown when mIRC returns an error response to a message.
    /// </summary>
    public class MircException : Exception {
        public MircErrorCode ErrorCode { get; }
        
        public MircException(MircErrorCode errorCode) : base("mIRC returned an error code: " + errorCode) {
            this.ErrorCode = errorCode;
        }
    }

    /// <summary>
    /// Represents an error code returned by mIRC when a command fails.
    /// </summary>
    [Flags]
    public enum MircErrorCode {
        /// <summary>The command completed successfully.</summary>
        Success = 0,
        /// <summary>The command failed.</summary>
        Error = 1,
        /// <summary>The mapped file name specified was invalid.</summary>
        BadMappedFileName = 2,
        BadMappedFileSize = 4,
        /// <summary>The event ID specified was invalid.</summary>
        BadEventID = 8,
        BadServer = 16,
        BadCode = 32,
        /// <summary>SendMessage commands are disabled in the Lock options.</summary>
        Disabled = 64
    }
#endif
}
