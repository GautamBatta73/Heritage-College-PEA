import React, { useState, useEffect } from 'react';
import '../Styles/App.css';
import List from './List';
import ActorSelect from './ActorSelect';
import YearSelect from './YearSelect';
import MovieAdd from './MovieAdd';
import TitleSelect from './TitleSelect';
import Logo from '../assets/logo.png';

if (sessionStorage.getItem('currentScreen') === null)
  sessionStorage.setItem('currentScreen', `HOME`);


function App() {
  const [movies, setMovies] = useState([]);
  const [currentScreen, setCurrentScreen] = useState("HOME");

  useEffect(() => setCurrentScreen(sessionStorage.getItem('currentScreen')), []);

  const choosePage = e => {
    sessionStorage.setItem('currentScreen', e.target.value);
    setCurrentScreen(e.target.value);
  }

  useEffect(() => {
    const fetchMovies = async () => {
      setMovies(await fetch('/movies')
        .then(resp => resp.json())
        .then(data => data));
    };
    fetchMovies();
  }, []);

  return (
    <>
      <header>
        <img src={Logo} alt="logo" />
        <nav>
          <button onClick={choosePage} value="HOME"
            className={currentScreen === "HOME" ? "selectedPage" : ""}>
            Home
          </button>
          <button onClick={choosePage} value="ACTOR"
            className={currentScreen === "ACTOR" ? "selectedPage" : ""}>
            Search By Actor
          </button>
          <button onClick={choosePage} value="YEAR"
            className={currentScreen === "YEAR" ? "selectedPage" : ""}>
            Search By Year
          </button>
          <button onClick={choosePage} value="TITLE"
            className={currentScreen === "TITLE" ? "selectedPage" : ""}>
            Search By Title
          </button>
          <button onClick={choosePage} value="ADD"
            className={currentScreen === "ADD" ? "selectedPage" : ""}>
            Add a Movie
          </button>
        </nav>
      </header>

      {console.log(currentScreen)}
      {(currentScreen === "HOME") ? (
        movies.length ? (
          <>
            <h1>List of All Movies</h1>
            <div className='searchResult'>
              <List movies={movies} id="M" />
            </div>
          </>
        ) : (
          <></>
        )
      ) : (
        currentScreen === "ACTOR" ? (
          <ActorSelect />
        ) : (
          currentScreen === "YEAR" ? (
            <YearSelect />
          ) : (
            currentScreen === "TITLE" ? (
              <TitleSelect />
            ) : (
              <MovieAdd />
            )
          )
        )
      )}
    </>
  );
}

export default App;