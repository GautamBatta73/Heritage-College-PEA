import React, { useState } from 'react';
import { IdContext } from './IdContext';
import Movie from './Movie';

export default function List(props) {
    const [ID, setID] = useState(0);

    function infoTogggle(e, movie) {
        if (ID === 0 || ID !== movie.Key) {
            setID(movie.Key);
        } else {
            setID(0);
        }
    }

    return (
        <IdContext.Provider value={ID}>
            {props.movies.length ? (
                <>
                    {props.movies.map((movie) => (
                        <div className={`movieDiv-${ID === movie.Key ? `open` : `closed`}`} key={`${props.id}${movie.Key}`} onClick={(e) => infoTogggle(e, movie)}>
                            Movie: {movie.Title}
                            {props.noYear ?
                                <></> :
                                <>
                                    <br />
                                    &emsp;Year: {movie.Year}
                                </>
                            }
                            {ID === movie.Key && <Movie />}
                        </div>
                    ))}
                </>
            ) : (
                <>
                    <h4>No movies found</h4>
                </>
            )}
        </IdContext.Provider>
    );
}