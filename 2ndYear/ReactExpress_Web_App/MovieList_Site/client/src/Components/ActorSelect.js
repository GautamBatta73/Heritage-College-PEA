import React, { useState } from 'react';
import List from './List';

export default function ActorSelect() {
    const [movies, setMovies] = useState([]);

    const fetchMovies = async (name) => {
        try {
            const response = await fetch(`/actors/${name}`);
            if (!response.ok) {
                throw new Error(`Error fetching movies for actor ${name}`);
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
            <h1>Search For Movies By Actor</h1>
            <div className='inputSearch'>
                <input type="text" name="actorInput" id="actorInput" />
                <button
                    className='searchBtn'
                    onClick={() => {
                        let val = document.querySelector("#actorInput").value;
                        fetchMovies(val);
                    }}>Search</button>
            </div>
            {movies.length ? (
                <div className='searchResult'>
                    <List movies={movies} id="A" />
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