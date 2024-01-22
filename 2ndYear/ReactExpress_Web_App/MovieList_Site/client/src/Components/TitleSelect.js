import React, { useState } from 'react';
import List from './List';

export default function TitleSelect() {
    const [movies, setMovies] = useState([]);

    const fetchMovies = async (name) => {
        try {
            const response = await fetch(`/movies/${name}`);
            if (!response.ok) {
                throw new Error(`Error fetching movies for movie ${name}`);
            }
            const data = await response.json();
            setMovies(data);
        } catch (error) {
            console.error(error);
            setMovies([]);
        }
    };
    return (
        <div>
            <h1>Search For Movies By Title</h1>
            <div className='inputSearch'>
                <input type="text" name="titleInput" id="titleInput" />
                <button
                    className='searchBtn'
                    onClick={() => {
                        let val = document.querySelector("#titleInput").value;
                        fetchMovies(val);
                    }}>Search</button>
            </div>
            {movies.length ? (
                <div className='searchResult'>
                    <List movies={movies} id="T" />
                </div>
            ) : (
                <div className='searchResult'>
                    <p>No Results Found...</p>
                </div>
            )}
            <br />
        </div>
    )
}