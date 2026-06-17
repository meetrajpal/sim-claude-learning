const fs = require("fs");
const path = require("path");

let data = "";
process.stdin.on("data", chunk => data += chunk);
process.stdin.on("end", () => {
  const { tool_name, tool_input, session_id } = JSON.parse(data);

  const log = `[${new Date().toISOString()}] PRE | Tool: ${tool_name} | Input: ${JSON.stringify(tool_input)} | Session: ${session_id}\n`;

  fs.mkdirSync("logs", { recursive: true });
  fs.appendFileSync(path.join("logs", "mcp-audit.log"), log);

  process.exit(0); // allow
});