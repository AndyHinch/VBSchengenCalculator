import { TripDate } from './tripDate';

/**
 * Collection of trips with serialisation support.
 * Ported from TripDates.vb
 */
export class TripDates {
  private _entries: TripDate[] = [];

  get entries(): TripDate[] {
    return [...this._entries];
  }

  addEntry(trip: TripDate): void {
    this._entries.push(trip);
  }

  /** Removes the trip whose start date matches exactly. */
  removeEntry(startDate: Date): void {
    this._entries = this._entries.filter(
      (t) => t.startDate.getTime() !== startDate.getTime()
    );
  }

  removeAll(): void {
    this._entries = [];
  }

  maxDate(): Date {
    if (this._entries.length === 0) return new Date();
    return this._entries.reduce(
      (max, t) => (t.endDate > max ? t.endDate : max),
      this._entries[0].endDate
    );
  }

  minDate(): Date {
    if (this._entries.length === 0) return new Date();
    return this._entries.reduce(
      (min, t) => (t.startDate < min ? t.startDate : min),
      this._entries[0].startDate
    );
  }

  toJson(): string {
    return JSON.stringify(
      this._entries.map((t) => ({
        startDate: t.startDate.toISOString(),
        endDate: t.endDate.toISOString(),
      }))
    );
  }

  /**
   * Loads trips from JSON.
   * Handles both camelCase keys (React output) and PascalCase keys
   * (Newtonsoft.Json output from the original VB.NET app).
   */
  loadFromJson(json: string): void {
    const raw = JSON.parse(json) as Array<Record<string, string>>;
    this._entries = raw.map((d) => {
      const start = new Date(d.startDate ?? d.StartDate);
      const end = new Date(d.endDate ?? d.EndDate);
      return new TripDate(start, end);
    });
  }
}
