using CA.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Log2Markdown
{
    public class App
    {
        private static readonly Dictionary<string, (string alias, string description, Action<string> action)> CommandList
            = new Dictionary<string, (string alias, string description, Action<string> action)>()
            {
                { "--help", ("h", "Show a list of all commands.", (input) => HandleHelp()) },
                { "--reset", ("rst", "Reset to default all loaded logs (free memory after this process).", (input) => HandleReset()) },
                { "--has-logs", ("hl", "Check if contains logs files (*.txt) from directory.", (input) => HandleHasLogs(input)) },
                { "--load", ("ld", "Load a single log file (*.txt) from path.", (input) => HandleLoad(input)) },
                { "--load-all", ("ld-all", "Load all log files (*.txt) from directory. You can use argument -D to include subdirectories.", (input) => HandleLoadAll(input)) },
                { "--error", ("er", "Serialize all log files of type ERROR.", (input) => HandleError()) },
                { "--fatal", ("ft", "Serialize all log files of type FATAL.", (input) => HandleFatal()) },
                { "--all", ("", "Serialize all log files of type ERROR and FATAL at same time.", (input) => HandleAll()) },
                { "--dump", ("dmp", "Generates a file that contains a Markdown file (*.md).", (input) => HandleDump()) },
                { "--view-error", ("v-er", "Display all formatted exception of type ERROR.", (input) => HandleViewError()) },
                { "--view-fatal", ("v-ft", "Display all formatted exception of type FATAL.", (input) => HandleViewFatal()) },
                { "--view-error-id", ("v-er-id", "Get all information about specific Exception Entry of type ERROR.", (input) => HandleViewErrorId(input)) },
                { "--view-fatal-id", ("v-ft-id", "Get all information about specific Exception Entry of type FATAL.", (input) => HandleViewFatalId(input)) }
            };

        private static Dictionary<string, string> Commands = new Dictionary<string, string>();
        private static Dictionary<string, string> Aliases = new Dictionary<string, string>();
        private static string[] ErrorLogs = new[] { "" };
        private static string[] FatalLogs = new[] { "" };
        private static List<ExceptionEntry> ErrorExceptions = new List<ExceptionEntry>();
        private static List<ExceptionEntry> FatalExceptions = new List<ExceptionEntry>();
        private static float CurrentLine { get; set; }
        private static float LastLine { get; set; }

        private static void Main()
        {
            var name = Assembly.GetExecutingAssembly().GetName().Name;
            var version =
                $"{Assembly.GetExecutingAssembly().GetName().Version}".Substring(0,
                $"{Assembly.GetExecutingAssembly().GetName().Version}".Length - 2);

            Console.Title = $"{name} v{version} - A TO-DO list generator for GitHub issue.";

            var mre = new ManualResetEvent(false);

            Console.CancelKeyPress += delegate { mre.Set(); };

            Warn("Initializing...");

            Task.Factory.StartNew(Core, TaskCreationOptions.AttachedToParent);

            mre.WaitOne();

            Warn("Terminating...");

            Thread.Sleep(500);
            Environment.Exit(0);
        }

        #region "Core"

        private static void Core()
        {
            Info("Loading commands and aliases...");

            foreach (var entry in CommandList)
            {
                var command = entry.Key;
                var alias = $"--{entry.Value.alias}";

                Commands.Add(command, alias);
                Aliases.Add(alias, command);
            }

            Info($"Loaded {CommandList.Count} command{(CommandList.Count > 1 ? "s" : "")}.");
            Info("Log2Markdown is ready to use!");
            Breakline();
            Warn("Press ANY key to continue...");

            Console.ReadKey(true);

            CoreLoop();
        }

        private static void CoreLoop()
        {
            Console.Clear();

            DisplayTitle();
            Breakline();
            Breakline();
            Info("Type --help for more details.");
            Breakline();

            var input = Console.ReadLine();
            var args = input.Split(' ');

            if (args.Length == 0)
            {
                Breakline();

                Console.Clear();

                CoreLoop();
                return;
            }

            var command = args[0];
            var newInput = string.Join(" ", args.Skip(1));

            Console.Clear();

            DisplayTitle();
            Breakline();
            Breakline();

            if (Commands.ContainsKey(command)) CommandList[command].action(newInput);
            else if (Aliases.ContainsKey(command)) CommandList[Aliases[command]].action(newInput);
            else CoreLoop();
        }

        #endregion "Core"

        #region "Command Handlers"

        private static void HandleHelp()
        {
            Info("Log2Markdown is a developer tool to convert TK log pattern to a formatted TO-DO list for GitHub issue.");
            Breakline();

            var entryId = 1;

            foreach (var entry in CommandList)
                Info(string.Format(
                    "[{0}]. {1}{2}\n\t{3}\n",
                    entryId++,
                    entry.Key.Remove(0, 2),
                    $" (alias: {entry.Value.alias})" ?? "",
                    entry.Value.description
                ));

            Warn("Press ANY key to continue...");

            Console.ReadKey(true);

            CoreLoop();
        }

        private static void HandleReset()
        {
            ErrorLogs = new[] { "" };
            FatalLogs = new[] { "" };
            ErrorExceptions.Clear();
            FatalExceptions.Clear();

            Info("All logs have been wiped!");
            Tail();
        }

        private static void HandleHasLogs(string input)
        {
            if (!Directory.Exists(input))
            {
                Error($"There is no directory for following path: {input}");
                Tail();
                return;
            }

            var fileNames = new[] { "error.txt", "fatal.txt" };
            bool hasFile(string file) => File.Exists(Path.Combine(input, file));

            Info($"From directory: {input}");

            for (var i = 0; i < fileNames.Length; i++)
                Info($"[{(hasFile(fileNames[i]) ? "X" : " ")}] /{fileNames[i]}");

            Tail();
        }

        private static void HandleLoad(string input)
        {
            var exist = File.Exists(input);

            if (!exist || string.IsNullOrWhiteSpace(input))
            {
                Error($"There is such file on following path: {input}");
                Tail();
                return;
            }

            var fileName = Path.GetFileNameWithoutExtension(input);

            Info($"From path: {input}");
            Info($"\t[{(exist ? "X" : " ")}] /{fileName}.txt");
            Breakline();

            string[] updateLog() => File.ReadAllLines(input);
            var updatedLog = updateLog();

            if (fileName.Contains("error"))
            {
                if (ErrorLogs[0] != "")
                {
                    var newErrorLogs = new string[ErrorLogs.Length + updatedLog.Length];
                    ErrorLogs.CopyTo(newErrorLogs, 0);
                    updatedLog.CopyTo(newErrorLogs, ErrorLogs.Length);

                    ErrorLogs = newErrorLogs;
                }
                else ErrorLogs = updatedLog;
            }

            if (fileName.Contains("fatal"))
            {
                if (FatalLogs[0] != "")
                {
                    var newFatalLogs = new string[FatalLogs.Length + updatedLog.Length];
                    FatalLogs.CopyTo(newFatalLogs, 0);
                    updatedLog.CopyTo(newFatalLogs, FatalLogs.Length);

                    FatalLogs = newFatalLogs;
                }
                else FatalLogs = updatedLog;
            }

            Info($"Loaded log: /{fileName}.txt");
            Tail();
        }

        private static void HandleLoadAll(string input)
        {
            string[] updateLog(string file, bool useInput = true)
                => useInput
                    ? File.ReadAllLines(Path.Combine(input, file))
                    : File.ReadAllLines(file);

            if (input.StartsWith("-D "))
            {
                var dir = input.Remove(0, 3);

                if (!Directory.Exists(dir) || string.IsNullOrWhiteSpace(dir))
                {
                    Error($"There is no directory for following path: {dir}");
                    Tail();
                    return;
                }

                var errorFiles = Directory.GetFiles(dir, "error.txt", SearchOption.AllDirectories);
                var fatalFiles = Directory.GetFiles(dir, "fatal.txt", SearchOption.AllDirectories);

                Info($"From directories:");

                for (var i = 0; i < errorFiles.Length; i++)
                    Info($"\t[X] {errorFiles[i]}");

                for (var j = 0; j < fatalFiles.Length; j++)
                    Info($"\t[X] {fatalFiles[j]}");

                Breakline();

                for (var k = 0; k < errorFiles.Length; k++)
                {
                    var updatedLog = updateLog(errorFiles[k], false);

                    if (ErrorLogs[0] != "")
                    {
                        var newErrorLogs = new string[ErrorLogs.Length + updatedLog.Length];
                        ErrorLogs.CopyTo(newErrorLogs, 0);
                        updatedLog.CopyTo(newErrorLogs, ErrorLogs.Length);

                        ErrorLogs = newErrorLogs;
                    }
                    else ErrorLogs = updatedLog;
                }

                for (var k = 0; k < fatalFiles.Length; k++)
                {
                    var updatedLog = updateLog(fatalFiles[k], false);

                    if (FatalLogs[0] != "")
                    {
                        var newFatalLogs = new string[FatalLogs.Length + updatedLog.Length];
                        FatalLogs.CopyTo(newFatalLogs, 0);
                        updatedLog.CopyTo(newFatalLogs, FatalLogs.Length);

                        FatalLogs = newFatalLogs;
                    }
                    else FatalLogs = updatedLog;
                }

#if DEBUG
                DisplayAllLogs();
#endif
                Tail();
                return;
            }

            if (!Directory.Exists(input) || string.IsNullOrWhiteSpace(input))
            {
                Error($"There is no directory for following path: {input}");
                Tail();
                return;
            }

            var files = new (string name, bool exist)[] {
                ("error.txt", false), ("fatal.txt", false)
            };
            bool hasFile(string file) => File.Exists(Path.Combine(input, file));

            Info($"From directory: {input}");

            for (var i = 0; i < files.Length; i++)
                Info($"\t[{((files[i].exist = hasFile(files[i].name)) ? "X" : " ")}] /{files[i].name}");

            Breakline();

            var existFiles = files.Where(file => file.exist).Select(file => file.name).ToArray();

            for (var j = 0; j < existFiles.Length; j++)
            {
                var updatedLog = updateLog(existFiles[j]);

                if (existFiles[j].Contains("error"))
                {
                    if (ErrorLogs[0] != "")
                    {
                        var newErrorLogs = new string[ErrorLogs.Length + updatedLog.Length];
                        ErrorLogs.CopyTo(newErrorLogs, 0);
                        updatedLog.CopyTo(newErrorLogs, ErrorLogs.Length);

                        ErrorLogs = newErrorLogs;
                    }
                    else ErrorLogs = updatedLog;
                }

                if (existFiles[j].Contains("fatal"))
                {
                    if (FatalLogs[0] != "")
                    {
                        var newFatalLogs = new string[FatalLogs.Length + updatedLog.Length];
                        FatalLogs.CopyTo(newFatalLogs, 0);
                        updatedLog.CopyTo(newFatalLogs, FatalLogs.Length);

                        FatalLogs = newFatalLogs;
                    }
                    else FatalLogs = updatedLog;
                }
            }
#if DEBUG
            DisplayAllLogs();
#endif

            Info($"Loaded {existFiles.Length} log{(existFiles.Length > 1 ? "s" : "")}.");
            Tail();
        }

        private static void HandleError(bool goToFatalHandlers = false)
        {
            if (ErrorLogs.Length == 1 && ErrorLogs[0] == "")
            {
                Error("There is no error log to process, consider to use following commands:" +
                    "\n\t--load  <path>" +
                    "\n\t--load-all <path>");
                Tail();
                return;
            }

            Info($"Serializing errors within {ErrorLogs.Length} line{(ErrorLogs.Length > 1 ? "s" : "")}...");

            var entryId = 0;
            var entries = new Dictionary<int, ExceptionEntry>();

            CurrentLine = 0;
            LastLine = ErrorLogs.Length;

            var source = new CancellationTokenSource();
            var progressRoutine = new InternalRoutine(200, GetProgressInfo());
            progressRoutine.AttachToParent(source.Token);
            progressRoutine.Start();

            for (var i = 0; i < ErrorLogs.Length; i++)
            {
                CurrentLine = i + 1;

                var line = ErrorLogs[i];

#if DEBUG
                Info(line);
#endif

                if (line.Contains("|ERROR|") && !entries.ContainsKey(entryId))
                {
                    var firstArgs = line.Split('|');
                    var level = firstArgs[1];
                    var package = firstArgs[2];
                    var exception = "UnknownException: ";

                    if (firstArgs[3].Contains("System."))
                        exception = firstArgs[3].Split(' ').First(pattern => pattern.Contains("System."));
                    else
                        exception += firstArgs[3];

                    var exceptionEntry = new ExceptionEntry(level, package, exception, null, null, null);

                    entries.Add(entryId, exceptionEntry);
                    continue;
                }

                if (!line.StartsWith("   at ") || !entries.ContainsKey(entryId)) continue;

                var innerArgs = line.Replace("   at ", "");

                string method, fileArgs;
                var fileName = string.Empty;
                var fileLine = "unknown line";

                if (innerArgs.Contains(" in "))
                {
                    var newInnerArgs = innerArgs.Replace(" in ", "$").Split('$');

                    method = newInnerArgs[0];
                    fileArgs = newInnerArgs[1];

                    if (fileArgs.Contains(":line "))
                    {
                        fileArgs = fileArgs.Replace(":line ", "$");

                        var args = fileArgs.Split('$');

                        fileName = args[0];
                        fileLine = args[1];
                    }
                    else fileName = fileArgs;
                }
                else
                {
                    method = innerArgs;
                    fileName = "unknown name";
                }

                var entry = entries[entryId];
                entry.method = method;
                entry.name = fileName;
                entry.line = fileLine;

                entries[entryId] = entry;

                entryId++;
            }

            source.Cancel();

            Thread.Sleep(500);

            GetProgressInfo().Invoke();
            Info($"Loaded {entries.Count} log exception{(entries.Count > 1 ? "s" : "")}.");
            ErrorExceptions.AddRange(entries.Values);
            Tail(!goToFatalHandlers);
        }

        private static void HandleFatal()
        {
            if (FatalLogs.Length == 1 && FatalLogs[0] == "")
            {
                Error("There is no fatal log to process, consider to use following commands:" +
                    "\n\t--load  <path>" +
                    "\n\t--load-all <path>");
                Tail();
                return;
            }

            Info($"Serializing errors within {FatalLogs.Length} line{(FatalLogs.Length > 1 ? "s" : "")}...");

            var entryId = 0;
            var entries = new Dictionary<int, ExceptionEntry>();

            CurrentLine = 0;
            LastLine = FatalLogs.Length;

            var source = new CancellationTokenSource();
            var progressRoutine = new InternalRoutine(200, GetProgressInfo());
            progressRoutine.AttachToParent(source.Token);
            progressRoutine.Start();

            for (var i = 0; i < FatalLogs.Length; i++)
            {
                CurrentLine = i + 1;

                var line = FatalLogs[i];

#if DEBUG
                Info(line);
#endif

                if (line.Contains("|FATAL|") && !entries.ContainsKey(entryId))
                {
                    var firstArgs = line.Split('|');
                    var level = firstArgs[1];
                    var package = firstArgs[2];
                    var exception = "UnknownException: ";

                    if (firstArgs[3].Contains("System."))
                        exception = firstArgs[3].Split(' ').First(pattern => pattern.Contains("System."));
                    else
                        exception += firstArgs[3];

                    var exceptionEntry = new ExceptionEntry(level, package, exception, null, null, null);

                    entries.Add(entryId, exceptionEntry);
                    continue;
                }

                if (!line.StartsWith("   at ") || !entries.ContainsKey(entryId)) continue;

                var innerArgs = line.Replace("   at ", "");

                string method, fileArgs;
                var fileName = string.Empty;
                var fileLine = "unknown line";

                if (innerArgs.Contains(" in "))
                {
                    var newInnerArgs = innerArgs.Replace(" in ", "$").Split('$');

                    method = newInnerArgs[0];
                    fileArgs = newInnerArgs[1];

                    if (fileArgs.Contains(":line "))
                    {
                        fileArgs = fileArgs.Replace(":line ", "$");

                        var args = fileArgs.Split('$');

                        fileName = args[0];
                        fileLine = args[1];
                    }
                    else fileName = fileArgs;
                }
                else
                {
                    method = innerArgs;
                    fileName = "unknown name";
                }

                var entry = entries[entryId];
                entry.method = method;
                entry.name = fileName;
                entry.line = fileLine;

                entries[entryId] = entry;

                entryId++;
            }

            source.Cancel();

            Thread.Sleep(500);

            GetProgressInfo().Invoke();
            Info($"Loaded {entries.Count} log exception{(entries.Count > 1 ? "s" : "")}.");
            FatalExceptions.AddRange(entries.Values);
            Tail();
        }

        private static void HandleAll()
        {
            HandleError(true);
            HandleFatal();
        }

        private static void HandleDump()
        {
            var areErrorsEmpty = ErrorExceptions.Count == 0;
            var areFatalsEmpty = FatalExceptions.Count == 0;

            if (areErrorsEmpty && areFatalsEmpty)
            {
                Error("There is none exception entry to dump.");
                Tail();
                return;
            }

            var contents = string.Empty;
            var mdHeader = GenerateHeader();
            var mdErrorHeader = GenerateExceptionHeader("Error");
            var mdFatalHeader = GenerateExceptionHeader("Fatal");
            var mdErrorList = new List<string>();
            var mdFatalList = new List<string>();

            if (!areErrorsEmpty)
            {
                Info("Processing error logs...");

                var errorsDistinct = ErrorExceptions.Distinct().ToArray();

                Info($"Removed duplicated error entries, found {ErrorExceptions.Count - errorsDistinct.Count()}...");

                for (var i = 0; i < errorsDistinct.Length; i++)
                    mdErrorList.Add(GenerateExceptionItem(i, errorsDistinct[i]));
            }

            if (!areFatalsEmpty)
            {
                Info("Processing fatal logs...");

                var fatalsDistinct = FatalExceptions.Distinct().ToArray();

                Info($"Removed duplicated fatal error entries, found {FatalExceptions.Count - fatalsDistinct.Count()}...");

                for (var i = 0; i < fatalsDistinct.Length; i++)
                    mdFatalList.Add(GenerateExceptionItem(i, fatalsDistinct[i]));
            }

            var mdStatistics = GenerateStatistics(mdErrorList.Count, mdFatalList.Count);

            contents += mdHeader + mdStatistics + mdErrorHeader;
            if (mdErrorList.Count != 0) contents += string.Join("", mdErrorList);
            else contents +=
                    @"None.
---

"
                ;

            contents += mdFatalHeader;

            if (mdFatalList.Count != 0) contents += string.Join("", mdFatalList);
            else contents += "None.";

            var utc = DateTime.UtcNow;
            var path =
                $"[L2M] GitHub TO-DO List " +
                $"({utc.ToString().Replace('/', '-').Replace(':', '.')}).md";

            Info($"Saving file to path: /{path}");

            File.WriteAllText(path, contents);

            Tail();
        }

        private static void HandleViewError()
        {
            if (ErrorLogs.Length == 1 && ErrorLogs[0] == "")
            {
                Error("There is no error log to process, consider to use following commands:" +
                    "\n\t--load  <path>" +
                    "\n\t--load-all <path>");
                Tail();
                return;
            }

            Info(
                $"Displaying {ErrorExceptions.Count} ERROR exception entr" +
                $"{(ErrorExceptions.Count > 1 ? "ies" : "y")} errors from log with " +
                $"{ErrorLogs.Length} line{(ErrorLogs.Length > 1 ? "s" : "")}..."
            );
            Breakline();

            string nullProperties;

            for (var i = 0; i < ErrorExceptions.Count; i++)
            {
                var error = ErrorExceptions[i];

                if ((nullProperties = ErrorExceptions[i].GetNullProperties()) != null)
                    Warn($"[ERROR] Exception Entry ID {i} has null inputs, see: {nullProperties}.");

                Info($"[ERROR] Exception Entry ID {i}:\n{error.ToString()}");
            }

            Tail();
        }

        private static void HandleViewFatal()
        {
            if (FatalLogs.Length == 1 && FatalLogs[0] == "")
            {
                Error("There is no fatal log to process, consider to use following commands:" +
                    "\n\t--load  <path>" +
                    "\n\t--load-all <path>");
                Tail();
                return;
            }

            Info(
                $"Displaying {FatalExceptions.Count} FATAL exception entr" +
                $"{(FatalExceptions.Count > 1 ? "ies" : "y")} fatal errors from log with " +
                $"{FatalLogs.Length} line{(FatalLogs.Length > 1 ? "s" : "")}..."
            );
            Breakline();

            string nullProperties;

            for (var i = 0; i < FatalExceptions.Count; i++)
            {
                var fatal = FatalExceptions[i];

                if ((nullProperties = FatalExceptions[i].GetNullProperties()) != null)
                    Warn($"[FATAL] Exception Entry ID {i} has null inputs, see: {nullProperties}.");

                Info($"[FATAL] Exception Entry ID {i}:\n{fatal.ToString()}");
            }

            Tail();
        }

        private static void HandleViewErrorId(string input)
        {
            if (ErrorLogs.Length == 1 && ErrorLogs[0] == "")
            {
                Error("There is no error log to process, consider to use following commands:" +
                    "\n\t--load  <path>" +
                    "\n\t--load-all <path>");
                Tail();
                return;
            }

            if (!int.TryParse(input, out int index))
            {
                Error("Invalid ID.");
                Tail();
                return;
            }

            if (index < 0 || index >= ErrorExceptions.Count)
            {
                Error(
                    $"ID is invalid, valid ID" +
                    $"{(ErrorExceptions.Count > 1 ? $"s: 0 to {ErrorExceptions.Count - 1}" : ": 0")}."
                );
                Tail();
                return;
            }

            var errorException = ErrorExceptions[index];

            Info($"Displaying ERROR exception entry for ID {index}:");
            Breakline();

            if (errorException.GetNullProperties() != null)
            {
                Breakline();
                Warn($"[ERROR] Exception Entry ID {index} has null inputs, see: {errorException.GetNullProperties()}.");
            }

            Info($"[ERROR] Exception Entry ID {index}:\n{errorException.ToString()}");
            Tail();
        }

        private static void HandleViewFatalId(string input)
        {
            if (FatalLogs.Length == 1 && FatalLogs[0] == "")
            {
                Error("There is no fatal log to process, consider to use following commands:" +
                    "\n\t--load  <path>" +
                    "\n\t--load-all <path>");
                Tail();
                return;
            }

            if (!int.TryParse(input, out int index))
            {
                Error("Invalid ID.");
                Tail();
                return;
            }

            if (index < 0 || index >= FatalExceptions.Count)
            {
                Error(
                    $"ID is invalid, valid ID" +
                    $"{(FatalExceptions.Count > 1 ? $"s: 0 to {FatalExceptions.Count - 1}" : ": 0")}."
                );
                Tail();
                return;
            }

            var errorException = FatalExceptions[index];

            Info($"Displaying FATAL exception entry for ID {index}:");
            Breakline();

            if (errorException.GetNullProperties() != null)
            {
                Breakline();
                Warn($"[FATAL] Exception Entry ID {index} has null inputs, see: {errorException.GetNullProperties()}.");
            }

            Info($"[FATAL] Exception Entry ID {index}:\n{errorException.ToString()}");
            Tail();
        }

        #endregion "Command Handlers"

        private static Action GetProgressInfo()
            => () => Info($"[{((CurrentLine / LastLine) * 100f).ToString("00.##")}%] " +
                $"{CurrentLine} of {LastLine} pending lines");

        private static string GenerateHeader()
            => string.Format(
@"# Log2Markdown (L2M)
> This to-do list was auto-generated by **Log2Markdown**, a developer tool to convert TK log pattern to a formatted TO-DO list for GitHub issue.

Log analysis made at **{0} UTC**.

---

",
                DateTime.UtcNow.ToString()
            );

        private static string GenerateExceptionHeader(string type)
            => string.Format(
@"### {0}
> Contains detailed information about all log entries of type `{0}`, without duplication (values are distinct).

",
                type, type.ToUpper()
            );

        private static string GenerateExceptionItem(int id, ExceptionEntry entry)
            => string.Format(
@"- [ ] **{0}ID-{1}**
```csharp
   - level: ""{2}""
   - package: ""{3}""
   - exception: ""{4}""
   - method: ""{5}""
   - name: ""{6}""
   - line: {7}
```

---

",
                    entry.level[0], id, entry.level, entry.package,
                    entry.exception, entry.method, entry.name, entry.line
                );

        private static string GenerateStatistics(int errorEntries, int fatalEntries)
            => string.Format(
@"### :bug: Bug Statistics
- **Error analysis**: detected **{0}** error entr{1} in logs.
- **Fatal analysis**: detected **{2}** fatal error entr{3} in logs.

---

",
                errorEntries, errorEntries > 1 ? "ies" : "y",
                fatalEntries, fatalEntries > 1 ? "ies" : "y"
            );

        private static void DisplayTitle()
            => Warn(
@"
__/\\\________________/\\\\\\\\\______/\\\\____________/\\\\_
 _\/\\\______________/\\\///////\\\___\/\\\\\\________/\\\\\\_
  _\/\\\_____________\///______\//\\\__\/\\\//\\\____/\\\//\\\_
   _\/\\\_______________________/\\\/___\/\\\\///\\\/\\\/_\/\\\_
    _\/\\\____________________/\\\//_____\/\\\__\///\\\/___\/\\\_
     _\/\\\_________________/\\\//________\/\\\____\///_____\/\\\_
      _\/\\\_______________/\\\/___________\/\\\_____________\/\\\_
       _\/\\\\\\\\\\\\\\\__/\\\\\\\\\\\\\\\_\/\\\_____________\/\\\_
        _\///////////////__\///////////////__\///______________\///__
"
            );

        private static void DisplayErrorsLog()
        {
            Info("Displaying errors:");

            for (var k = 0; k < ErrorLogs.Length; k++)
                Warn($"[line {k + 1}] {ErrorLogs[k]}");

            Breakline();
        }

        private static void DisplayFatalsLog()
        {
            Info("Displaying fatal errors:");

            for (var l = 0; l < FatalLogs.Length; l++)
                Error($"[line {l + 1}] {FatalLogs[l]}");

            Breakline();
        }

        private static void DisplayAllLogs()
        {
            DisplayErrorsLog();
            DisplayFatalsLog();
        }

        #region "Log utilities"

        private static void Tail(bool goToMenu = true)
        {
            Breakline();

            if (goToMenu)
            {
                Warn("Press ANY key to continue...");

                Console.ReadKey(true);

                CoreLoop();
            }
        }

        private static void Breakline() => Console.WriteLine("\n");

        private static void Info(string message) => Log(message, ConsoleColor.Gray);

        private static void Warn(string message) => Log(message, ConsoleColor.DarkYellow);

        private static void Error(string message) => Log(message, ConsoleColor.DarkRed);

        private static void Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        #endregion "Log utilities"
    }
}
