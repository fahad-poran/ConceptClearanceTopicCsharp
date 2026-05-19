import { inferWhereItAppears } from "../utils/courseLocations";
import { CodeWalkthroughList } from "./CodeWalkthroughList";

export function LessonPane({ topic, activeIndex, total }) {
  return (
    <section className="panel lesson-pane" aria-live="polite">
      <div className="lesson-progress">
        <span className="lesson-progress__step">Lesson {activeIndex + 1}</span>
        <span className="lesson-progress__count">{activeIndex + 1} of {total}</span>
      </div>
      <h2>{topic.title}</h2>
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

      <article className="lesson-block">
        <h3>Step-by-step guide</h3>
        <ol className="lesson-steps">
          {topic.steps.map((step) => (
            <li key={step}>{step}</li>
          ))}
        </ol>
      </article>

      <article className="lesson-block">
        <h3>Code walkthrough</h3>
        <CodeWalkthroughList walkthroughs={topic.codeWalkthroughs ?? []} />
      </article>

      <article className="lesson-block lesson-block--recap">
        <h3>Quick recap</h3>
        <p className="muted">{topic.recap}</p>
      </article>
    </section>
  );
}
