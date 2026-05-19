import { useEffect, useState } from "react";
import { loadInterviewContent } from "./api/interviewPrepApi";
import { CourseSection } from "./components/CourseSection";
import { Hero } from "./components/Hero";
import { OrderCalculator } from "./components/OrderCalculator";
import { Panel } from "./components/Panel";

export default function App() {
  const [overview, setOverview] = useState({
    title: "Project Overview",
    summary: ""
  });
  const [course, setCourse] = useState([]);
  const [loadError, setLoadError] = useState("");

  useEffect(() => {
    let isMounted = true;

    loadInterviewContent()
      .then((content) => {
        if (!isMounted) {
          return;
        }

        setOverview(content.overview);
        setCourse(content.course);
      })
      .catch(() => {
        if (isMounted) {
          setLoadError("Content failed to load. Start the API host and refresh.");
        }
      });

    return () => {
      isMounted = false;
    };
  }, []);

  return (
    <>
      <Hero />
      <main className="layout">
        <Panel className="panel--intro">
          <h2>{overview.title}</h2>
          <p className="muted">{loadError || overview.summary}</p>
        </Panel>

        <CourseSection topics={course} />
        <OrderCalculator />
      </main>
    </>
  );
}
