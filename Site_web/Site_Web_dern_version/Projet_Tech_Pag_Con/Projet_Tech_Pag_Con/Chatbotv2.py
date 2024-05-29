import json
from sentence_transformers import SentenceTransformer
import numpy as np
import spacy
from sklearn.metrics.pairwise import cosine_similarity
nlp = spacy.load('en_core_web_md')

# Charger le modèle Sentence-BERT
model = SentenceTransformer('all-MiniLM-L6-v2')

def load_knowledge_base(file_path: str) -> dict:
    try:
        with open(file_path, 'r') as file:
            return json.load(file)
    except (FileNotFoundError, json.JSONDecodeError) as e:
        print(f"Error loading knowledge base: {e}")
        return {"questions": []}

def save_knowledge_base(file_path: str, data: dict):
    try:
        with open(file_path, 'w') as file:
            json.dump(data, file, indent=2)
    except IOError as e:
        print(f"Error saving knowledge base: {e}")

def preprocess_text(text: str) -> str:
    doc = nlp(text.lower())
    return " ".join([token.lemma_ for token in doc if not token.is_stop])

def get_vector(text: str) -> np.ndarray:
    processed_text = preprocess_text(text)
    return model.encode(processed_text)

def find_best_match(user_question: str, questions: list[str]) -> str | None:
    user_vector = get_vector(user_question)
    question_vectors = [get_vector(q) for q in questions]
    similarities = cosine_similarity([user_vector], question_vectors)[0]
    best_index = np.argmax(similarities)
    print(f"User question: '{user_question}' - Best match: '{questions[best_index]}' with similarity: {similarities[best_index]}")
    return questions[best_index] if similarities[best_index] > 0.6 else None

def get_answer_for_question(question: str, knowledge_base: dict) -> str | None:
    for q in knowledge_base.get("questions", []):
        if q.get("question") == question:
            return q.get("answer")
    return None