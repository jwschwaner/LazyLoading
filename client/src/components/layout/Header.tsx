import React from 'react';
import '../../styles/layout/Header.css';

const Header: React.FC = () => {
  return (
    <header className="app-header">
      <h1 className="app-title">
        <span className="football-icon">⚽</span>
        LazyLoading
      </h1>
      <p className="app-subtitle">Infinite scroll matches with beautiful design</p>
    </header>
  );
};

export default Header;
