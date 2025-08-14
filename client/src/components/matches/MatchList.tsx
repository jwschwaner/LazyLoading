import React, { useCallback } from 'react';
import { useMatchItems } from '../../hooks/useMatches';
import MatchItem from './MatchItem';
import '../../styles/components/MatchList.css';

const MatchList: React.FC = () => {
  const { items, fetchNextPage, hasNextPage, isFetchingNextPage } = useMatchItems();

  const onScroll = useCallback((e: React.UIEvent<HTMLDivElement>) => {
    const { scrollTop, clientHeight, scrollHeight } = e.currentTarget;
    const threshold = scrollHeight * 0.9; // Load more when 90% scrolled (10% remaining)
    if (scrollTop + clientHeight > threshold && hasNextPage && !isFetchingNextPage) {
      fetchNextPage();
    }
  }, [fetchNextPage, hasNextPage, isFetchingNextPage]);

  return (
    <div className="matches-container">
      <div className="matches-header">
        <h2 className="matches-title">
          Football Matches ({items.length} {items.length === 1 ? 'match' : 'matches'})
        </h2>
      </div>

      <div className="matches-list" onScroll={onScroll}>
        {items.length === 0 ? (
          <div className="empty-state">
            <div className="empty-state-title">No matches found</div>
            <div className="empty-state-text">
              Use the "Seed Matches" button above to generate some sample football matches.
            </div>
          </div>
        ) : (
          <>
            {items.map(match => (
              <MatchItem key={match.id} match={match} />
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
  );
};

export default MatchList;
