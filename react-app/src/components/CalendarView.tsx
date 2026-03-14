import { useState } from 'react';
import { SchengenCalculator } from '../lib/schengenCalculator';

const DAY_NAMES = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
const MONTH_NAMES = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

interface Props {
  getCalc: () => SchengenCalculator;
  maxDays: number;
}

export default function CalendarView({ getCalc, maxDays }: Props) {
  const today = new Date();
  const [viewYear, setViewYear] = useState(today.getFullYear());
  const [viewMonth, setViewMonth] = useState(today.getMonth());
  const [selected, setSelected] = useState<Date | null>(null);
  const [selectedInfo, setSelectedInfo] = useState('');
  const [selectedOver, setSelectedOver] = useState(false);

  function prevMonth() {
    if (viewMonth === 0) { setViewYear(y => y - 1); setViewMonth(11); }
    else setViewMonth(m => m - 1);
  }

  function nextMonth() {
    if (viewMonth === 11) { setViewYear(y => y + 1); setViewMonth(0); }
    else setViewMonth(m => m + 1);
  }

  function handleDayClick(date: Date) {
    setSelected(date);
    const calc = getCalc();
    const used = calc.numberOfDaysInAreaOnDay(date);
    const remaining = maxDays - used;
    if (remaining >= 0) {
      setSelectedInfo(`${used} used, ${remaining} remaining`);
      setSelectedOver(false);
    } else {
      setSelectedInfo(`${used} used, ${Math.abs(remaining)} over!`);
      setSelectedOver(true);
    }
  }

  // Build the grid of days for the current month view
  const firstDay = new Date(viewYear, viewMonth, 1).getDay();
  const daysInMonth = new Date(viewYear, viewMonth + 1, 0).getDate();

  const cells: (Date | null)[] = [];
  for (let i = 0; i < firstDay; i++) cells.push(null);
  for (let d = 1; d <= daysInMonth; d++) cells.push(new Date(viewYear, viewMonth, d));

  // Pad to complete last row
  while (cells.length % 7 !== 0) cells.push(null);

  function isToday(date: Date) {
    return (
      date.getFullYear() === today.getFullYear() &&
      date.getMonth() === today.getMonth() &&
      date.getDate() === today.getDate()
    );
  }

  function isSelected(date: Date) {
    return (
      selected !== null &&
      date.getFullYear() === selected.getFullYear() &&
      date.getMonth() === selected.getMonth() &&
      date.getDate() === selected.getDate()
    );
  }

  return (
    <section className="card calendar-card">
      <h2>Calendar View</h2>
      <p className="calendar-hint">Click a date to see days used.</p>

      <div className="calendar-nav">
        <button className="btn btn-icon" onClick={prevMonth}>&#8249;</button>
        <span className="calendar-month-label">
          {MONTH_NAMES[viewMonth]} {viewYear}
        </span>
        <button className="btn btn-icon" onClick={nextMonth}>&#8250;</button>
      </div>

      <div className="calendar-grid">
        {DAY_NAMES.map((d) => (
          <div key={d} className="calendar-day-name">{d}</div>
        ))}
        {cells.map((date, i) => {
          if (!date) return <div key={`empty-${i}`} className="calendar-cell empty" />;
          return (
            <div
              key={i}
              className={[
                'calendar-cell',
                isToday(date) ? 'today' : '',
                isSelected(date) ? 'selected' : '',
              ].join(' ')}
              onClick={() => handleDayClick(date)}
            >
              {date.getDate()}
            </div>
          );
        })}
      </div>

      {selectedInfo && (
        <p className={`calc-result ${selectedOver ? 'result-over' : 'result-ok'}`}>
          {selected?.toLocaleDateString()}: {selectedInfo}
        </p>
      )}
    </section>
  );
}
