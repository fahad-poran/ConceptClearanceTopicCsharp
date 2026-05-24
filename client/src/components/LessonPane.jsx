import { useEffect, useMemo, useState } from "react";
import { inferWhereItAppears } from "../utils/courseLocations";
import { CodeWalkthroughList } from "./CodeWalkthroughList";

export function LessonPane({ topic, activeIndex, total }) {
  const [pageIndex, setPageIndex] = useState(0);

  const pages = useMemo(() => {
    const walkthroughs = topic.codeWalkthroughs ?? [];

    return [
      {
        id: "overview",
        label: "Overview",
        content: (
          <>
            <p className="lesson-definition">{topic.definition}</p>
            <p className="lesson-description">{topic.description}</p>

            <div className="lesson-grid">
              <article className="lesson-block">
                <h3>Why it matters</h3>
                <p className="muted">{topic.description}</p>
              </article>
              <article className="lesson-block">
                <h3>Where it appears</h3>
                <p className="muted">{inferWhereItAppears(topic.id)}</p>
              </article>
            </div>
          </>
        )
      },
      {
        id: "guide",
        label: "Guide",
        content: (
          <article className="lesson-block">
            <h3>Step-by-step guide</h3>
            <ol className="lesson-steps">
              {topic.steps.map((step) => (
                <li key={step}>{step}</li>
              ))}
            </ol>
          </article>
        )
      },
      ...walkthroughs.map((walkthrough, index) => ({
        id: `walkthrough-${walkthrough.title}`,
        label: walkthroughs.length > 1 ? `Code ${index + 1}` : "Code",
        content: (
          <article className="lesson-block">
            <h3>Code walkthrough</h3>
            <CodeWalkthroughList walkthroughs={[walkthrough]} />
          </article>
        )
      })),
      {
        id: "recap",
        label: "Recap",
        content: (
          <article className="lesson-block lesson-block--recap">
            <h3>Quick recap</h3>
            <p className="muted">{topic.recap}</p>
          </article>
        )
      }
    ];
  }, [topic]);

  useEffect(() => {
    setPageIndex(0);
  }, [topic.id]);

  const activePage = pages[pageIndex] ?? pages[0];
  const isFirstPage = pageIndex === 0;
  const isLastPage = pageIndex === pages.length - 1;

  return (
    <section className="panel lesson-pane" aria-live="polite">
      <div className="lesson-progress">
        <span className="lesson-progress__step">Lesson {activeIndex + 1}</span>
        <span className="lesson-progress__count">{activeIndex + 1} of {total}</span>
      </div>
      <h2>{topic.title}</h2>

      <nav className="lesson-pages" aria-label="Lesson pages">
        {pages.map((page, index) => (
          <button
            key={page.id}
            className={`lesson-pages__tab ${index === pageIndex ? "is-active" : ""}`}
            type="button"
            onClick={() => setPageIndex(index)}
          >
            {page.label}
          </button>
        ))}
      </nav>

      <div className="lesson-page">
        {activePage.content}
      </div>

      <div className="lesson-pager">
        <button
          className="btn btn--ghost"
          type="button"
          onClick={() => setPageIndex((current) => Math.max(current - 1, 0))}
          disabled={isFirstPage}
        >
          Previous
        </button>
        <span className="lesson-pager__count">Page {pageIndex + 1} of {pages.length}</span>
        <button
          className="btn btn--primary"
          type="button"
          onClick={() => setPageIndex((current) => Math.min(current + 1, pages.length - 1))}
          disabled={isLastPage}
        >
          Next
        </button>
      </div>
    </section>
  );
}
