export function Hero() {
  return (
    <header className="hero">
      <div className="hero__content">
        <p className="eyebrow">Beginner-friendly course path</p>
        <h1>.NET 8 Interview Prep Studio</h1>
        <p className="hero__sub">
          Follow a guided lesson flow instead of a mixed dashboard. Each topic explains the definition,
          why it matters, where it appears in the project, and the steps to understand it.
        </p>
        <div className="hero__actions">
          <a href="#course" className="btn btn--primary">Start the course</a>
          <a href="#calculator" className="btn btn--ghost">Try the calculator</a>
        </div>
      </div>
    </header>
  );
}
