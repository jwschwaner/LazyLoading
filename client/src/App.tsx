import React from 'react';
import Header from './components/layout/Header';
import MatchControls from './components/matches/MatchControls';
import MatchList from './components/matches/MatchList';
import './styles/base.css';

const App: React.FC = () => {
  return (
    <div className="app-container">
      <Header />
      <MatchControls />
      <MatchList />
    </div>
  );
};

export default App;