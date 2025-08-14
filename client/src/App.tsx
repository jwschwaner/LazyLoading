import { useCallback, useMemo, useState } from 'react'
import { useInfiniteQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { deleteAllMatches, fetchMatches, seedMatches } from './api/matches'
import './App.css'

function useMatches() {
  return useInfiniteQuery({
    queryKey: ['matches'],
    queryFn: ({ pageParam }) => fetchMatches(pageParam ?? null, 10),
    getNextPageParam: (last) => last.nextCursor ?? undefined,
    initialPageParam: null as string | null,
    staleTime: 60_000,
  })
}

export default function App() {
  const { data, fetchNextPage, hasNextPage, isFetchingNextPage } = useMatches()
  const items = useMemo(() => (data?.pages ?? []).flatMap(p => p.items), [data])

  const [count, setCount] = useState('100')
  const qc = useQueryClient()

  const seed = useMutation({
    mutationFn: async () => seedMatches(parseInt(count || '0', 10) || 0),
    onSuccess: async () => { await qc.invalidateQueries({ queryKey: ['matches'] }); },
  })

  const clearAll = useMutation({
    mutationFn: deleteAllMatches,
    onSuccess: async () => { await qc.resetQueries({ queryKey: ['matches'] }); },
  })

  const onScroll = useCallback((e: any) => {
    const { scrollTop, clientHeight, scrollHeight } = e.currentTarget
    const threshold = scrollHeight * 0.9 // Load more when 90% scrolled (10% remaining)
    if (scrollTop + clientHeight > threshold && hasNextPage && !isFetchingNextPage) {
      fetchNextPage()
    }
  }, [fetchNextPage, hasNextPage, isFetchingNextPage])

  return (
    <div className="app-container">
      <header className="app-header">
        <h1 className="app-title">⚽ LazyLoading</h1>
        <p className="app-subtitle">Infinite scroll matches with beautiful design</p>
      </header>

      <div className="controls-container">
        <div className="controls-row">
          <span className="controls-label">Generate matches:</span>
          <input 
            type="number" 
            min={1} 
            step={1} 
            value={count} 
            onChange={(e) => setCount(e.target.value)}
            placeholder="Number of matches"
          />
          <button 
            onClick={() => seed.mutate()} 
            disabled={seed.isPending}
            className="btn-success"
          >
            {seed.isPending ? 'Seeding...' : `Seed ${count} Matches`}
          </button>
          <button 
            onClick={() => clearAll.mutate()} 
            disabled={clearAll.isPending}
            className="btn-danger"
          >
            {clearAll.isPending ? 'Clearing...' : 'Delete All'}
          </button>
        </div>
      </div>

      <div className="matches-container">
        <div className="matches-header">
          <h2 className="matches-title">
            Football Matches ({items.length} {items.length === 1 ? 'match' : 'matches'})
          </h2>
        </div>

        <div className="matches-list" onScroll={onScroll}>
          {items.length === 0 && data ? (
            <div className="empty-state">
              <div className="empty-state-title">No matches found</div>
              <div className="empty-state-text">
                Use the "Seed Matches" button above to generate some sample football matches.
              </div>
            </div>
          ) : (
            <>
              {items.map(m => (
                <div key={m.id} className="match-item">
                  <div className="match-header">
                    <div className="match-teams">{m.homeTeam} vs {m.awayTeam}</div>
                    <div className="match-score">{m.homeScore} - {m.awayScore}</div>
                  </div>
                  <div className="match-date">
                    🗓️ {new Date(m.kickoffAt).toLocaleDateString('en-US', {
                      weekday: 'long',
                      year: 'numeric',
                      month: 'long',
                      day: 'numeric',
                      hour: '2-digit',
                      minute: '2-digit'
                    })}
                  </div>
                </div>
              ))}

              {isFetchingNextPage && (
                <div className="loading-indicator">
                  ⚽ Loading more matches...
                </div>
              )}

              {!hasNextPage && items.length > 0 && (
                <div className="end-indicator">
                  🏁 You've reached the end of all matches
                </div>
              )}
            </>
          )}
        </div>
      </div>
    </div>
  )
}