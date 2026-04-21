import { useState } from 'react';
import { TripDates } from '../lib/tripDates';
import { SchengenCalculator } from '../lib/schengenCalculator';

interface Props {
  tripDates: TripDates;
  getCalc: () => SchengenCalculator;
  maxDays: number;
  reviewPeriod: number;
}

interface Prediction {
  date: Date;
  used: number;
  remaining: number;
}

export default function Predictions({ tripDates, getCalc, maxDays, reviewPeriod }: Props) {
  const [predictions, setPredictions] = useState<Prediction[]>([]);

  function handleGenerate() {
    const entries = tripDates.entries;
    if (entries.length === 0) {
      setPredictions([]);
      return;
    }

    const calc = getCalc();

    // Start at beginning of the month containing minDate
    let minDate = tripDates.minDate();
    minDate = new Date(minDate.getFullYear(), minDate.getMonth(), 1);

    // End reviewPeriod days after the month following maxDate
    let maxDate = tripDates.maxDate();
    maxDate = new Date(maxDate.getFullYear(), maxDate.getMonth() + 1, 1);
    maxDate = new Date(maxDate.getFullYear(), maxDate.getMonth(), 1 + reviewPeriod);
    // Snap to start of that month
    maxDate = new Date(maxDate.getFullYear(), maxDate.getMonth(), 1);

    const results: Prediction[] = [];
    let current = new Date(minDate);

    while (current <= maxDate) {
      const used = calc.numberOfDaysInAreaOnDay(current);
      results.push({ date: new Date(current), used, remaining: maxDays - used });
      current.setMonth(current.getMonth() + 1);
    }

    setPredictions(results);
  }

  return (
    <section className="card">
      <h2>Monthly Predictions</h2>
      <button className="btn btn-primary" onClick={handleGenerate}>Generate Predictions</button>
      {predictions.length > 0 && (
        <ul className="predictions-list">
          {predictions.map((p, i) => (
            <li key={i} className={p.remaining < 0 ? 'pred-over' : 'pred-ok'}>
              <span className="pred-date">{p.date.toLocaleDateString()}</span>
              {p.remaining >= 0
                ? ` : ${p.used} used, ${p.remaining} remaining`
                : ` : ${p.used} used, ${Math.abs(p.remaining)} over!`}
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
