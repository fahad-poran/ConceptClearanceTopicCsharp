import { PrismLight as SyntaxHighlighter } from "react-syntax-highlighter";
import csharp from "react-syntax-highlighter/dist/esm/languages/prism/csharp";
import { oneDark } from "react-syntax-highlighter/dist/esm/styles/prism";

SyntaxHighlighter.registerLanguage("csharp", csharp);

export function CodeWalkthroughList({ walkthroughs }) {
  return (
    <div className="code-walkthroughs">
      {walkthroughs.map((walkthrough) => (
        <section className="code-walkthrough" key={walkthrough.title}>
          <h4>{walkthrough.title}</h4>
          <SyntaxHighlighter
            language="csharp"
            style={oneDark}
            customStyle={{
              margin: 0,
              borderRadius: "10px",
              border: "1px solid #c9d6e4",
              fontSize: "0.9rem",
              lineHeight: 1.55
            }}
            codeTagProps={{ style: { fontFamily: 'Consolas, "Courier New", monospace' } }}
            wrapLongLines={false}
          >
            {walkthrough.code.trim()}
          </SyntaxHighlighter>
          <div className="code-walkthrough__notes">
            <p><strong>What this code does</strong></p>
            <p>{walkthrough.explanation}</p>
            <p><strong>Why this code was used</strong></p>
            <p>{walkthrough.whyUsed}</p>
          </div>
        </section>
      ))}
    </div>
  );
}
