import { api } from "./client";
import type { MatchDto, PagedResponse } from "../types/match";

export async function fetchMatches(cursor?: string | null, limit = 10): Promise<PagedResponse<MatchDto>> {
  const params: Record<string, string | number> = { limit };
  if (cursor) params.cursor = cursor;
  const res = await api.get<PagedResponse<MatchDto>>('/api/matches', { params });
  return res.data;
}

export async function seedMatches(count = 100) {
  const res = await api.post('/api/matches/seed', null, { params: { count } });
  return res.data as { added: number };
}

export async function deleteAllMatches() {
  const res = await api.delete('/api/matches');
  return res.data as { deleted: number };
}