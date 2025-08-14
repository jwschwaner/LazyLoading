import React from 'react';
import type { MatchDto } from '../../types/match';
import '../../styles/components/MatchItem.css';

interface MatchItemProps {
  match: MatchDto;
}

const MatchItem: React.FC<MatchItemProps> = ({ match }) => {
  return (
    <div className="match-item">
      <div className="match-header">
        <div className="match-teams">{match.homeTeam} vs {match.awayTeam}</div>
        <div className="match-score">{match.homeScore} - {match.awayScore}</div>
      </div>
      <div className="match-date">
        🗓️ {new Date(match.kickoffAt).toLocaleDateString('en-US', {
          weekday: 'long',
          year: 'numeric',
          month: 'long',
          day: 'numeric',
          hour: '2-digit',
          minute: '2-digit'
        })}
      </div>
    </div>
  );
};

export default MatchItem;
