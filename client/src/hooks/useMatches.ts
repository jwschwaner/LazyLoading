import { useMemo } from 'react';
import { useMutation, useInfiniteQuery, useQueryClient } from '@tanstack/react-query';
import { deleteAllMatches, fetchMatches, seedMatches } from '../api/matches';

export function useMatches() {
  return useInfiniteQuery({
    queryKey: ['matches'],
    queryFn: ({ pageParam }) => fetchMatches(pageParam ?? null, 10),
    getNextPageParam: (last) => last.nextCursor ?? undefined,
    initialPageParam: null as string | null,
    staleTime: 60_000,
  });
}

export function useMatchItems() {
  const { data, ...rest } = useMatches();
  const items = useMemo(() => (data?.pages ?? []).flatMap(p => p.items), [data]);
  return { items, data, ...rest };
}

export function useSeedMatches() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: async (count: number) => seedMatches(count),
    onSuccess: async () => { 
      await queryClient.invalidateQueries({ queryKey: ['matches'] }); 
    },
  });
}

export function useDeleteAllMatches() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: deleteAllMatches,
    onSuccess: async () => { 
      await queryClient.resetQueries({ queryKey: ['matches'] }); 
    },
  });
}
