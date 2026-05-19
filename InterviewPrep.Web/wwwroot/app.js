const state = { items: [] };

const topics = [
  { title: "OOP", detail: "Payment hierarchy and processing behavior by type." },
  { title: "CTS", detail: "Value vs reference semantics, boxing/unboxing concepts." },
  { title: "Tuples", detail: "Discount engine returns amount + reason as a tuple." },
  { title: "Readonly", detail: "Order item modeled as readonly record struct." },
  { title: "Generics", detail: "Generic repository and utility methods for reusability." },
  { title: "Pattern Matching", detail: "Type and property pattern checks in payment logic." },
  { title: "Delegates/Events", detail: "Payment completion flow through event callbacks." },
  { title: ".NET 8 Features", detail: "FrozenDictionary, TimeProvider, primary constructors." }
];

const topicCards = document.getElementById("topicCards");
const itemsBody = document.getElementById("itemsBody");
const result = document.getElementById("result");
const apiBaseUrl = window.location.port === "5500"
  ? "http://127.0.0.1:5157"
  : window.location.origin;

function renderTopics() {
  topicCards.innerHTML = topics
    .map((t) => `<article class="card"><h3>${t.title}</h3><p>${t.detail}</p></article>`)
    .join("");
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

function formatCurrency(value) {
  return new Intl.NumberFormat(undefined, { style: "currency", currency: "USD" }).format(value);
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

renderTopics();
renderItems();
