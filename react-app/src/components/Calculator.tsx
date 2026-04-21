import { useState } from 'react';
import { SchengenCalculator } from '../lib/schengenCalculator';

function toInputValue(date: Date): string {
  return date.toISOString().split('T')[0];
}

interface Props {
  getCalc: () => SchengenCalculator;
}

export default function Calculator({ getCalc }: Props) {
  const [dateVal, setDateVal] = useState(toInputValue(new Date()));
  const [result, setResult] = useState('');
  const [isOver, setIsOver] = useState(false);

  function handleCalculate() {
    const reviewDate = new Date(dateVal + 'T00:00:00');
    const calc = getCalc();
    const used = calc.numberOfDaysInAreaOnDay(reviewDate);
    const remaining = calc.maxDays - used;
    if (remaining >= 0) {
      setResult(`${used} used, ${remaining} remaining`);
      setIsOver(false);
    } else {
      setResult(`${used} used, ${Math.abs(remaining)} over!`);
      setIsOver(true);
    }
  }

  return (
    <section className="card">
      <h2>Calculate Days</h2>
      <div className="calc-row">
        <label>Review Date</label>
        <input
          type="date"
          value={dateVal}
          onChange={(e) => setDateVal(e.target.value)}
        />
        <button className="btn btn-primary" onClick={handleCalculate}>Calculate</button>
      </div>
      {result && (
        <p className={`calc-result ${isOver ? 'result-over' : 'result-ok'}`}>
          {result}
        </p>
      )}
    </section>
  );
}
