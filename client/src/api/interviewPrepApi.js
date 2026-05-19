const apiBaseUrl = import.meta.env.DEV ? "" : window.location.origin;

async function readJson(response) {
  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  return response.json();
}

export async function loadInterviewContent() {
  const [overview, course] = await Promise.all([
    fetch(`${apiBaseUrl}/api/interview/overview`).then(readJson),
    fetch(`${apiBaseUrl}/api/interview/course`).then(readJson)
  ]);

  return { overview, course };
}

export async function calculateOrder(payload) {
  return fetch(`${apiBaseUrl}/api/order/calculate`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  }).then(readJson);
}
