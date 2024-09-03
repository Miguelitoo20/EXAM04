from flask import Flask, request, jsonify
import pandas as pd
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
from flask_cors import CORS

# Cargar el conjunto de datos de Pokémon
pokemon_df = pd.read_csv('pokemon_combined.csv')
pokemon_df['Description'] = pokemon_df['Name'] + " " + pokemon_df['Species'] + " " + pokemon_df['Type']

# Inicializar el vectorizador TF-IDF y ajustar los datos
tfidf_vectorizer = TfidfVectorizer()
tfidf_matrix = tfidf_vectorizer.fit_transform(pokemon_df['Description'])

# Crear la aplicación Flask
app = Flask(__name__)
CORS(app)  # Permitir solicitudes CORS

@app.route('/pokemons', methods=['GET'])
def get_pokemons():
    # Lógica para obtener la lista completa de Pokémon
    pokemons = pokemon_df[['Name', 'Type']].to_dict(orient='records')
    return jsonify(pokemons)

@app.route('/recommend', methods=['GET'])
def recommend_pokemon():
    name = request.args.get('name')
    
    if not name:
        return jsonify({"error": "Pokémon name is required."}), 400
    
    # Lógica para recomendar Pokémon basado en el nombre
    if name not in pokemon_df['Name'].values:
        return jsonify({"error": "Pokémon not found."}), 404
    
    index = pokemon_df[pokemon_df['Name'] == name].index[0]
    similarities = cosine_similarity(tfidf_matrix[index], tfidf_matrix).flatten()
    similar_indices = similarities.argsort()[-6:-1]  # Obtener los 5 más similares
    
    recommendations = pokemon_df.iloc[similar_indices][['Name', 'Type']].to_dict(orient='records')
    return jsonify(recommendations)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)  # Escuchar en todas las interfaces de red
