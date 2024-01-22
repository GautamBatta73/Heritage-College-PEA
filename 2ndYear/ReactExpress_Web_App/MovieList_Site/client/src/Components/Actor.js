import React, { useState, useEffect, useContext } from 'react';
import { IdContext } from './IdContext';

export default function Actor() {
    const id = useContext(IdContext);
    const [movie, setMovie] = useState([]);

    useEffect(() => {
        const fetchMovie = async () => {
            setMovie(await fetch(`/movies/${id}`)
                .then(resp => resp.json())
                .then(data => data));
        }
        fetchMovie();
    }, [id]);

    return (
        <>
            <br />
            &emsp;Actor(s):&nbsp;
            {movie.Actors && movie.Actors.length ? (
                <>
                    {movie.Actors.map((actor) => (
                        <>
                            <br />
                            &emsp;&emsp;{actor}
                        </>
                    ))}
                </>
            ) : (
                <>No actors found.</>
            )}
        </>
    )
}
