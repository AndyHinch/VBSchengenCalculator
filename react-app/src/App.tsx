import { useMemo, useState } from 'react';
import { TripDate } from './lib/tripDate';
import { TripDates } from './lib/tripDates';
import { SchengenCalculator } from './lib/schengenCalculator';
import TripList from './components/TripList';
import TripForm from './components/TripForm';
import Calculator from './components/Calculator';
import Predictions from './components/Predictions';
import CalendarView from './components/CalendarView';

const MAX_DAYS = 90;
const REVIEW_PERIOD = 180;

export default function App() {
  const [trips, setTrips] = useState<TripDate[]>([]);
  const [showForm, setShowForm] = useState(false);
  const [editingTrip, setEditingTrip] = useState<TripDate | null>(null);

  // Build TripDates and SchengenCalculator from current trips array
  const tripDates = useMemo(() => {
    const td = new TripDates();
    trips.forEach((t) => td.addEntry(t));
    return td;
  }, [trips]);

  function getCalc(): SchengenCalculator {
    return new SchengenCalculator(tripDates, MAX_DAYS, REVIEW_PERIOD);
  }

  // ---- Trip CRUD ----

  function handleSaveTrip(start: Date, end: Date) {
    if (editingTrip) {
      // Replace: remove old, add new
      setTrips((prev) => {
        const without = prev.filter(
          (t) => t.startDate.getTime() !== editingTrip.startDate.getTime()
        );
        return [...without, new TripDate(start, end)];
      });
    } else {
      setTrips((prev) => [...prev, new TripDate(start, end)]);
    }
    setShowForm(false);
    setEditingTrip(null);
  }

  function handleDelete(trip: TripDate) {
    setTrips((prev) =>
      prev.filter((t) => t.startDate.getTime() !== trip.startDate.getTime())
    );
  }

  function handleClearAll() {
    setTrips([]);
  }

  // ---- Save / Load ----

  function handleSave() {
    const json = tripDates.toJson();
    const blob = new Blob([json], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'trips.tripdata';
    a.click();
    URL.revokeObjectURL(url);
  }

  function handleLoad(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = (ev) => {
      try {
        const json = ev.target?.result as string;
        const td = new TripDates();
        td.loadFromJson(json);
        setTrips(td.entries);
      } catch {
        alert('Failed to load file. Please check the format.');
      }
    };
    reader.readAsText(file);
    // Reset input so the same file can be loaded again
    e.target.value = '';
  }

  return (
    <div className="app">
      <header className="app-header">
        <h1>Schengen Calculator</h1>
        <p className="subtitle">
          Track your 90-day / 180-day Schengen visa-free allowance
        </p>
      </header>

      <main className="app-main">
        <div className="top-panel">
          <TripList
            trips={trips}
            onAdd={() => { setEditingTrip(null); setShowForm(true); }}
            onEdit={(trip) => { setEditingTrip(trip); setShowForm(true); }}
            onDelete={handleDelete}
            onClearAll={handleClearAll}
            onSave={handleSave}
            onLoad={handleLoad}
          />
          <CalendarView getCalc={getCalc} maxDays={MAX_DAYS} />
        </div>

        <Calculator getCalc={getCalc} />
        <Predictions
          tripDates={tripDates}
          getCalc={getCalc}
          maxDays={MAX_DAYS}
          reviewPeriod={REVIEW_PERIOD}
        />
      </main>

      <footer className="app-footer">
        <p>© {new Date().getFullYear()} Andrew Hinchcliffe — MIT License</p>
      </footer>

      {showForm && (
        <TripForm
          initialStart={editingTrip?.startDate ?? new Date()}
          initialEnd={
            editingTrip?.endDate ??
            new Date(Date.now() + 14 * 24 * 60 * 60 * 1000)
          }
          onSave={handleSaveTrip}
          onCancel={() => { setShowForm(false); setEditingTrip(null); }}
        />
      )}
    </div>
  );
}
