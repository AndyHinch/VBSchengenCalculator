import { useState } from 'react';

function toInputValue(date: Date): string {
  return date.toISOString().split('T')[0];
}

interface Props {
  initialStart: Date;
  initialEnd: Date;
  onSave: (start: Date, end: Date) => void;
  onCancel: () => void;
}

export default function TripForm({ initialStart, initialEnd, onSave, onCancel }: Props) {
  const [startVal, setStartVal] = useState(toInputValue(initialStart));
  const [endVal, setEndVal] = useState(toInputValue(initialEnd));
  const [error, setError] = useState('');

  function handleSave() {
    const start = new Date(startVal + 'T00:00:00');
    const end = new Date(endVal + 'T00:00:00');
    if (start >= end) {
      setError('Start date must be before end date.');
      return;
    }
    onSave(start, end);
  }

  return (
    <div className="modal-backdrop">
      <div className="modal">
        <h2>Trip Dates</h2>
        <div className="form-row">
          <label>Start Date</label>
          <input
            type="date"
            value={startVal}
            onChange={(e) => { setStartVal(e.target.value); setError(''); }}
          />
        </div>
        <div className="form-row">
          <label>End Date</label>
          <input
            type="date"
            value={endVal}
            onChange={(e) => { setEndVal(e.target.value); setError(''); }}
          />
        </div>
        {error && <p className="error">{error}</p>}
        <div className="modal-actions">
          <button className="btn btn-primary" onClick={handleSave}>OK</button>
          <button className="btn" onClick={onCancel}>Cancel</button>
        </div>
      </div>
    </div>
  );
}
