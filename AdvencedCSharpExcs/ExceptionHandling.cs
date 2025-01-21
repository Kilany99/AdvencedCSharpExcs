using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{
    internal class ExceptionHandling
    {
    }


    //    Requirements:
    //- Create a FileProcessor that handles multiple file operations
    //- Implement custom exceptions for different error scenarios
    //- Use exception filters
    //- Implement proper cleanup using finally blocks
    //- Include retry logic for transient failures

    //Learning Objectives:
    //- Custom exception creation
    //- Exception handling patterns
    //- Resource cleanup
    //- Retry strategies

    public class FileProcessingException : Exception
    {
        public string FilePath { get; }
        public ProcessingStage Stage { get; }

        public FileProcessingException(string message, string filePath,
            ProcessingStage stage, Exception innerException) : base(message, innerException)
        {
            FilePath = filePath;
            Stage = stage;
        }

    }

    public class FileProcessor
    {
        private readonly ILogger<FileProcessor> _logger;
        private readonly string _outputDirectory;
        private int attempts = 0;
        public FileProcessor(ILogger<FileProcessor> logger,string dir)
        {
            _logger = logger; _outputDirectory = dir;
        }

        public async Task ProcessFileAsync(string inputPath,int retryCount = 3)
        {
            try
            {
                await ProcessFileInternalAsync(inputPath);
                return;
            }
            catch (Exception ex) when (IsTransientException(ex))
            {
                attempts++;
                if (attempts == retryCount)
                    throw new FileProcessingException(
                        "Max retry attempts reached",
                        inputPath,
                        ProcessingStage.Processing,
                        ex);

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempts))); // Exponential backoff
            }
        }

        private async Task ProcessFileInternalAsync(string inputPath)
        {
            string content = null;
            string outputPath = Path.Combine(_outputDirectory,
                $"processed_{Path.GetFileName(inputPath)}");

            try
            {
                // Read file
                content = await ReadFileWithRetryAsync(inputPath);

                // Validate content
                ValidateContent(content);

                // Process content
                string processedContent = await ProcessContentAsync(content);

                // Write processed content
                await WriteFileWithRetryAsync(outputPath, processedContent);

                _logger.LogInformation($"Successfully processed file: {inputPath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing file: {inputPath}");
                throw;
            }
            finally
            {
                // Cleanup if needed
                CleanupTempFiles(inputPath);
            }
        }
        private async Task<string> ReadFileWithRetryAsync(string path)
        {
            try
            {
                using var reader = new StreamReader(path);
                return await reader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                throw new FileProcessingException(
                    "Error reading file",
                    path,
                    ProcessingStage.Reading,
                    ex);
            }
        }
        private void ValidateContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new FileProcessingException(
                    "File content is empty",
                    string.Empty,
                    ProcessingStage.Validation,
                    null);

            // Add more validation as needed
        }

        private async Task<string> ProcessContentAsync(string content)
        {
            try
            {
                // Simulate processing
                await Task.Delay(100);
                return content.ToUpper();
            }
            catch (Exception ex)
            {
                throw new FileProcessingException(
                    "Error processing content",
                    string.Empty,
                    ProcessingStage.Processing,
                    ex);
            }
        }

        private async Task WriteFileWithRetryAsync(string path, string content)
        {
            try
            {
                await File.WriteAllTextAsync(path, content);
            }
            catch (Exception ex)
            {
                throw new FileProcessingException(
                    "Error writing file",
                    path,
                    ProcessingStage.Writing,
                    ex);
            }
        }

        private bool IsTransientException(Exception ex)
        {
            return ex is IOException || ex is TimeoutException;
        }

        private void CleanupTempFiles(string path)
        {
            throw new NotImplementedException();
        }

    }

    public enum ProcessingStage
    {
        Reading,
        Processing,
        Writing,
        Validation
    }

    // ... existing code ...

    public class MainClass2
    {
        public async Task Main2()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var logger = loggerFactory.CreateLogger<FileProcessor>();
            var processor = new FileProcessor(logger, "output");

            try
            {
                await processor.ProcessFileAsync("input.txt");
            }
            catch (FileProcessingException ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
                Console.WriteLine($"Stage: {ex.Stage}");
                Console.WriteLine($"File: {ex.FilePath}");
            }
        }
    }
}
