import { useState } from 'react';
import { TripDate } from '../lib/tripDate';

interface Props {
  trips: TripDate[];
  onAdd: () => void;
  onEdit: (trip: TripDate) => void;
  onDelete: (trip: TripDate) => void;
  onClearAll: () => void;
  onSave: () => void;
  onLoad: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

export default function TripList({ trips, onAdd, onEdit, onDelete, onClearAll, onSave, onLoad }: Props) {
  const [selected, setSelected] = useState<TripDate | null>(null);

  const sorted = [...trips].sort((a, b) => a.startDate.getTime() - b.startDate.getTime());

  function handleDelete() {
    if (selected) {
      onDelete(selected);
      setSelected(null);
    }
  }

  function handleEdit() {
    if (selected) onEdit(selected);
  }

  return (
    <section className="card trip-list-card">
      <h2>Trips</h2>
      <ul className="trip-list">
        {sorted.length === 0 && (
          <li className="trip-list-empty">No trips added yet.</li>
        )}
        {sorted.map((trip, i) => (
          <li
            key={i}
            className={`trip-item${selected === trip ? ' selected' : ''}`}
            onClick={() => setSelected(trip)}
          >
            {trip.toString()}
          </li>
        ))}
      </ul>

      <div className="button-row">
        <button className="btn btn-primary" onClick={onAdd}>Add</button>
        <button className="btn" onClick={handleEdit} disabled={!selected}>Edit</button>
        <button className="btn btn-danger" onClick={handleDelete} disabled={!selected}>Delete</button>
        <button className="btn btn-danger" onClick={() => { onClearAll(); setSelected(null); }}>
          Clear All
        </button>
      </div>

      <div className="button-row">
        <button className="btn" onClick={onSave} disabled={trips.length === 0}>
          Save (.tripdata)
        </button>
        <label className="btn" style={{ cursor: 'pointer' }}>
          Load (.tripdata)
          <input type="file" accept=".tripdata,.json" onChange={onLoad} style={{ display: 'none' }} />
        </label>
      </div>
    </section>
  );
}
