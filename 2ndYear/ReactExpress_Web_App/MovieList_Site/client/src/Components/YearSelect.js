import React, { useState } from 'react';
import List from './List';

export default function ActorSelect() {
    const [movies, setMovies] = useState([]);

    const fetchMovies = async (year) => {
        try {
            const response = await fetch(`/years/${year}`);
            if (!response.ok) {
                throw new Error(`Error fetching movies for ${year}`);
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
            <h1>Search For Movies By Year</h1>
            <div className='inputSearch'>
                <input type="text" name="yearInput" id="yearInput" />
                <button
                    className='searchBtn'
                    onClick={() => {
                        let val = document.querySelector("#yearInput").value;
                        fetchMovies(val);
                    }}>Search</button>
            </div>
            {movies.length ? (
                <div className='searchResult'>
                    <List movies={movies} id="Y" noYear />
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