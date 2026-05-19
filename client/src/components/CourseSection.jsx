import { useState } from "react";
import { CourseNav } from "./CourseNav";
import { LessonPane } from "./LessonPane";

export function CourseSection({ topics }) {
  const [activeIndex, setActiveIndex] = useState(0);
  const activeTopic = topics[activeIndex];

  if (topics.length === 0) {
    return (
      <section id="course" className="course-shell">
        <aside className="course-nav panel">
          <div className="course-nav__header">
            <h2>Course outline</h2>
            <p className="muted">Move through the topics in a steady learning order.</p>
          </div>
        </aside>
        <section className="panel lesson-pane" aria-live="polite">
          <p className="muted">Loading course lessons...</p>
        </section>
      </section>
    );
  }

  return (
    <section id="course" className="course-shell">
      <CourseNav topics={topics} activeIndex={activeIndex} onSelect={setActiveIndex} />
      <LessonPane topic={activeTopic} activeIndex={activeIndex} total={topics.length} />
    </section>
  );
}
