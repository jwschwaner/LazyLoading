export interface MatchDto {
  id: string;
  kickoffAt: string;
  homeTeam: string;
  awayTeam: string;
  homeScore: number;
  awayScore: number;
}

export interface PagedResponse<T> {
  items: T[]; 
  nextCursor?: string | null;
}
