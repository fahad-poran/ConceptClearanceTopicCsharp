const state = { items: [], course: [], activeCourseIndex: 0 };

const courseNav = document.getElementById("courseNav");
const lessonStep = document.getElementById("lessonStep");
const lessonCount = document.getElementById("lessonCount");
const lessonTitle = document.getElementById("lessonTitle");
const lessonDefinition = document.getElementById("lessonDefinition");
const lessonDescription = document.getElementById("lessonDescription");
const lessonWhy = document.getElementById("lessonWhy");
const lessonWhere = document.getElementById("lessonWhere");
const lessonSteps = document.getElementById("lessonSteps");
const lessonCodeWalkthroughs = document.getElementById("lessonCodeWalkthroughs");
const lessonRecap = document.getElementById("lessonRecap");
const itemsBody = document.getElementById("itemsBody");
const result = document.getElementById("result");
const overviewText = document.getElementById("overviewText");
const apiBaseUrl = window.location.port === "5500"
  ? "http://127.0.0.1:5157"
  : window.location.origin;

function formatCurrency(value) {
  return new Intl.NumberFormat(undefined, { style: "currency", currency: "USD" }).format(value);
}

function escapeHtml(value) {
  return String(value)
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#039;");
}

function renderItems() {
  itemsBody.innerHTML = state.items
    .map((item) => {
      const line = item.quantity * item.unitPrice;
      return `<tr>
        <td>${item.sku}</td>
        <td>${item.quantity}</td>
        <td>${formatCurrency(item.unitPrice)}</td>
        <td>${formatCurrency(line)}</td>
      </tr>`;
    })
    .join("");
}

function renderCourseNav() {
  courseNav.innerHTML = state.course
    .map((topic, index) => `
      <li>
        <button class="course-nav__item ${index === state.activeCourseIndex ? "is-active" : ""}" data-index="${index}">
          <span class="course-nav__index">${String(index + 1).padStart(2, "0")}</span>
          <span>
            <strong>${topic.title}</strong>
            <small>${topic.definition}</small>
          </span>
        </button>
      </li>
    `)
    .join("");

  courseNav.querySelectorAll("button[data-index]").forEach((button) => {
    button.addEventListener("click", () => {
      showLesson(Number(button.dataset.index));
    });
  });
}

function renderLesson(topic) {
  lessonStep.textContent = `Lesson ${state.activeCourseIndex + 1}`;
  lessonCount.textContent = `${state.activeCourseIndex + 1} of ${state.course.length}`;
  lessonTitle.textContent = topic.title;
  lessonDefinition.textContent = topic.definition;
  lessonDescription.textContent = topic.description;
  lessonWhy.textContent = topic.description;
  lessonWhere.textContent = inferWhereItAppears(topic.id);
  lessonSteps.innerHTML = topic.steps.map((step) => `<li>${escapeHtml(step)}</li>`).join("");
  lessonCodeWalkthroughs.innerHTML = (topic.codeWalkthroughs ?? [])
    .map((walkthrough) => `
      <section class="code-walkthrough">
        <h4>${escapeHtml(walkthrough.title)}</h4>
        <pre><code>${escapeHtml(walkthrough.code.trim())}</code></pre>
        <div class="code-walkthrough__notes">
          <p><strong>What this code does</strong></p>
          <p>${escapeHtml(walkthrough.explanation)}</p>
          <p><strong>Why this code was used</strong></p>
          <p>${escapeHtml(walkthrough.whyUsed)}</p>
        </div>
      </section>
    `)
    .join("");
  lessonRecap.textContent = topic.recap;
  renderCourseNav();
}

function inferWhereItAppears(topicId) {
  switch (topicId) {
    case "oop":
      return "Domain/Payment.cs and Services/PaymentProcessor.cs";
    case "cts":
      return "The shared domain and service layer, where type behavior becomes visible";
    case "tuples":
      return "Services/OrderCalculationService.cs";
    case "readonly":
      return "Domain/OrderItem.cs";
    case "generics":
      return "Common/Repository.cs and reusable utility patterns";
    case "pattern-matching":
      return "Services/PaymentProcessor.cs";
    case "delegates-events":
      return "Services/PaymentProcessor.cs and the code that subscribes to completion events";
    case "dotnet8":
      return "Services/InventoryService.cs and the app startup configuration";
    default:
      return "The shared lesson code";
  }
}

function showLesson(index) {
  state.activeCourseIndex = index;
  renderLesson(state.course[index]);
}

async function loadInterviewContent() {
  const [overview, course] = await Promise.all([
    fetch(`${apiBaseUrl}/api/interview/overview`).then((r) => r.json()),
    fetch(`${apiBaseUrl}/api/interview/course`).then((r) => r.json())
  ]);

  document.getElementById("overviewTitle").textContent = overview.title;
  overviewText.textContent = overview.summary;
  state.course = course;
  renderCourseNav();
  showLesson(0);
}

function readItemFromForm() {
  const sku = document.getElementById("sku").value.trim().toUpperCase();
  const quantity = Number(document.getElementById("quantity").value);
  const unitPrice = Number(document.getElementById("unitPrice").value);

  if (!sku || quantity <= 0 || unitPrice <= 0) {
    throw new Error("Provide valid SKU, quantity, and unit price.");
  }

  return { sku, quantity, unitPrice };
}

document.getElementById("addItemBtn").addEventListener("click", () => {
  try {
    const item = readItemFromForm();
    state.items.push(item);
    renderItems();
    result.style.display = "none";
  } catch (err) {
    alert(err.message);
  }
});

document.getElementById("clearBtn").addEventListener("click", () => {
  state.items = [];
  renderItems();
  result.style.display = "none";
});

document.getElementById("orderForm").addEventListener("submit", async (event) => {
  event.preventDefault();

  const customerName = document.getElementById("customerName").value.trim();
  if (!customerName) {
    alert("Customer name is required.");
    return;
  }

  if (state.items.length === 0) {
    try {
      state.items.push(readItemFromForm());
      renderItems();
    } catch (err) {
      alert("Add at least one valid item before calculation.");
      return;
    }
  }

  const payload = { customerName, items: state.items };

  const response = await fetch(`${apiBaseUrl}/api/order/calculate`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  });

  if (!response.ok) {
    alert("Calculation failed. Please try again.");
    return;
  }

  const data = await response.json();

  result.innerHTML = `
    <strong>Request:</strong> ${data.requestId}<br>
    <strong>Accepted Items:</strong> ${data.acceptedItems.length}<br>
    <strong>Gross:</strong> ${formatCurrency(data.grossTotal)}<br>
    <strong>Discount:</strong> ${formatCurrency(data.discountAmount)} (${data.discountReason})<br>
    <strong>Final:</strong> ${formatCurrency(data.finalTotal)}<br>
    <strong>Generated (UTC):</strong> ${new Date(data.generatedAtUtc).toISOString()}
  `;
  result.style.display = "block";
});

loadInterviewContent().catch(() => {
  overviewText.textContent = "Content failed to load. Start the API host and refresh.";
});
renderItems();
