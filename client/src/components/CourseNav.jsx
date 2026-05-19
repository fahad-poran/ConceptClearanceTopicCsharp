export function CourseNav({ topics, activeIndex, onSelect }) {
  return (
    <aside className="course-nav panel">
      <div className="course-nav__header">
        <h2>Course outline</h2>
        <p className="muted">Move through the topics in a steady learning order.</p>
      </div>
      <ol className="course-nav__list" aria-label="Course navigation">
        {topics.map((topic, index) => (
          <li key={topic.id}>
            <button
              className={`course-nav__item ${index === activeIndex ? "is-active" : ""}`}
              type="button"
              onClick={() => onSelect(index)}
            >
              <span className="course-nav__index">{String(index + 1).padStart(2, "0")}</span>
              <span>
                <strong>{topic.title}</strong>
                <small>{topic.definition}</small>
              </span>
            </button>
          </li>
        ))}
      </ol>
    </aside>
  );
}
