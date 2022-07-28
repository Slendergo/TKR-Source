using System.Collections.Generic;

namespace Log2Markdown
{
    public struct ExceptionEntry
    {
        public string level;
        public string package;
        public string exception;
        public string method;
        public string name;
        public string line;

        public ExceptionEntry(
            string level,
            string package,
            string exception,
            string method,
            string name,
            string line
            )
        {
            this.level = level ?? "unknown";
            this.package = package ?? "unknown";
            this.exception = exception ?? "unknown";
            this.method = method ?? "unknown";
            this.name = name ?? "unknown";
            this.line = line ?? "unknown";
        }

        public override string ToString()
            => string.Format(
                "  level: {0},\n" +
                "  package: {1},\n" +
                "  exception: {2},\n" +
                "  method: {3},\n" +
                "  name: {4},\n" +
                "  line: {5}",
                level, package, exception, method, name, line
            );

        public string GetNullProperties()
        {
            var nullProperties = new List<string>();

            if (level == null) nullProperties.Add("level");
            if (package == null) nullProperties.Add("package");
            if (exception == null) nullProperties.Add("exception");
            if (method == null) nullProperties.Add("method");
            if (name == null) nullProperties.Add("name");
            if (line == null) nullProperties.Add("line");

            if (nullProperties.Count == 1) return nullProperties[0];

            return nullProperties.Count > 1
                ? string.Join(", ", nullProperties)
                : null;
        }
    }
}