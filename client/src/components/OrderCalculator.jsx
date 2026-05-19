import { useState } from "react";
import { calculateOrder } from "../api/interviewPrepApi";
import { formatCurrency } from "../utils/formatters";

const initialDraft = {
  customerName: "Alice Johnson",
  sku: "LAPTOP-001",
  quantity: 1,
  unitPrice: 1200
};

export function OrderCalculator() {
  const [draft, setDraft] = useState(initialDraft);
  const [items, setItems] = useState([]);
  const [result, setResult] = useState(null);

  function updateField(event) {
    const { id, value } = event.target;
    setDraft((current) => ({
      ...current,
      [id]: id === "quantity" || id === "unitPrice" ? Number(value) : value
    }));
  }

  function readItem() {
    const sku = draft.sku.trim().toUpperCase();
    const quantity = Number(draft.quantity);
    const unitPrice = Number(draft.unitPrice);

    if (!sku || quantity <= 0 || unitPrice <= 0) {
      throw new Error("Provide valid SKU, quantity, and unit price.");
    }

    return { sku, quantity, unitPrice };
  }

  function addItem() {
    try {
      setItems((current) => [...current, readItem()]);
      setResult(null);
    } catch (err) {
      alert(err.message);
    }
  }

  function clearItems() {
    setItems([]);
    setResult(null);
  }

  async function submitOrder(event) {
    event.preventDefault();

    const customerName = draft.customerName.trim();
    if (!customerName) {
      alert("Customer name is required.");
      return;
    }

    let orderItems = items;
    if (orderItems.length === 0) {
      try {
        orderItems = [readItem()];
        setItems(orderItems);
      } catch {
        alert("Add at least one valid item before calculation.");
        return;
      }
    }

    try {
      const data = await calculateOrder({ customerName, items: orderItems });
      setResult(data);
    } catch {
      alert("Calculation failed. Please try again.");
    }
  }

  return (
    <section id="calculator" className="panel">
      <h2>Real-World Order Calculator</h2>
      <p className="muted">Use sample SKUs (`LAPTOP-001`, `MOUSE-010`, `BAG-200`) to see live discount logic.</p>

      <form className="form-grid" onSubmit={submitOrder}>
        <label htmlFor="customerName">
          Customer Name
          <input type="text" id="customerName" value={draft.customerName} onChange={updateField} required />
        </label>

        <label htmlFor="sku">
          SKU
          <input type="text" id="sku" value={draft.sku} onChange={updateField} required />
        </label>

        <label htmlFor="quantity">
          Quantity
          <input type="number" id="quantity" value={draft.quantity} min="1" onChange={updateField} required />
        </label>

        <label htmlFor="unitPrice">
          Unit Price
          <input type="number" id="unitPrice" value={draft.unitPrice} min="1" step="0.01" onChange={updateField} required />
        </label>

        <div className="actions">
          <button type="button" className="btn btn--secondary" onClick={addItem}>Add Item</button>
          <button type="submit" className="btn btn--primary">Calculate</button>
          <button type="button" className="btn btn--ghost" onClick={clearItems}>Clear</button>
        </div>
      </form>

      <div className="table-wrap">
        <table>
          <thead>
            <tr>
              <th>SKU</th>
              <th>Quantity</th>
              <th>Unit Price</th>
              <th>Line Total</th>
            </tr>
          </thead>
          <tbody>
            {items.map((item, index) => (
              <tr key={`${item.sku}-${index}`}>
                <td>{item.sku}</td>
                <td>{item.quantity}</td>
                <td>{formatCurrency(item.unitPrice)}</td>
                <td>{formatCurrency(item.quantity * item.unitPrice)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {result && (
        <div className="result" aria-live="polite">
          <strong>Request:</strong> {result.requestId}<br />
          <strong>Accepted Items:</strong> {result.acceptedItems.length}<br />
          <strong>Gross:</strong> {formatCurrency(result.grossTotal)}<br />
          <strong>Discount:</strong> {formatCurrency(result.discountAmount)} ({result.discountReason})<br />
          <strong>Final:</strong> {formatCurrency(result.finalTotal)}<br />
          <strong>Generated (UTC):</strong> {new Date(result.generatedAtUtc).toISOString()}
        </div>
      )}
    </section>
  );
}
