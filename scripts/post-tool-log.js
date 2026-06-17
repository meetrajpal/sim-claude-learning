const fs = require("fs");
const path = require("path");

let data = "";
process.stdin.on("data", chunk => data += chunk);
process.stdin.on("end", () => {
  const { tool_name, tool_result, session_id } = JSON.parse(data);

  const log = `[${new Date().toISOString()}] POST | Tool: ${tool_name} | Result: ${JSON.stringify(tool_result)} | Session: ${session_id}\n`;

  fs.mkdirSync("logs", { recursive: true });
  fs.appendFileSync(path.join("logs", "mcp-results.log"), log);

  process.exit(0);
});