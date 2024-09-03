import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [pokemonName, setPokemonName] = useState('');
  const [recommendations, setRecommendations] = useState([]);
  const [pokemons, setPokemons] = useState([]);
  const [error, setError] = useState('');

  const fetchPokemons = async () => {
    try {
      const response = await axios.get('/pokemons');
      setPokemons(response.data);
    } catch (error) {
      setError('Error fetching Pokémon data');
      console.error("Error fetching Pokémon data", error);
    }
  };

  const handleRecommendation = async () => {
    if (!pokemonName) {
      setError('Please enter a Pokémon name.');
      return;
    }

    try {
      const response = await axios.get(`/recommend?name=${pokemonName}`);
      setRecommendations(response.data);
      setError('');
    } catch (error) {
      setError('Error getting recommendations.');
      console.error("Error getting recommendations", error);
    }
  };

  return (
    <div className="App">
      <h1>Pokémon Recommendation</h1>
      <input
        type="text"
        placeholder="Enter Pokémon Name"
        value={pokemonName}
        onChange={(e) => setPokemonName(e.target.value)}
      />
      <button onClick={handleRecommendation}>Get Recommendations</button>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <h2>Recommendations:</h2>
      <ul>
        {recommendations.map((pokemon, index) => (
          <li key={index}>{pokemon.Name} - {pokemon.Type}</li>
        ))}
      </ul>
      <button onClick={fetchPokemons}>Fetch All Pokémon</button>
      <h2>All Pokémon:</h2>
      <ul>
        {pokemons.map((pokemon, index) => (
          <li key={index}>{pokemon.Name} - {pokemon.Type}</li>
        ))}
      </ul>
    </div>
  );
}

export default App;