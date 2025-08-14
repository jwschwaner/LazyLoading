import React, { useState } from 'react';
import { useDeleteAllMatches, useSeedMatches } from '../../hooks/useMatches';
import '../../styles/components/MatchControls.css';

const MatchControls: React.FC = () => {
  const [count, setCount] = useState('100');
  const seedMutation = useSeedMatches();
  const clearAllMutation = useDeleteAllMatches();

  const handleSeed = () => {
    seedMutation.mutate(parseInt(count || '0', 10) || 0);
  };

  return (
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
          onClick={handleSeed} 
          disabled={seedMutation.isPending}
          className="btn-success"
        >
          {seedMutation.isPending ? 'Seeding...' : `Seed ${count} Matches`}
        </button>
        <button 
          onClick={() => clearAllMutation.mutate()} 
          disabled={clearAllMutation.isPending}
          className="btn-danger"
        >
          {clearAllMutation.isPending ? 'Clearing...' : 'Delete All'}
        </button>
      </div>
    </div>
  );
};

export default MatchControls;
