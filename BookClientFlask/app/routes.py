from app import app
from flask import render_template, jsonify, request, session, redirect, url_for
import requests

@app.route('/')
def client():
    return render_template('index.html')

@app.route('/create_user', methods=['GET', 'POST'])
def create_user():
    if request.method == 'POST':
        name = request.form["name"]
        username = request.form['username']
        email = request.form['email']
        password = request.form['password']

        api_url = 'http://localhost:5287/api/User/register'
        data = {
            'name': name,
            'userName': username,
            'email': email,
            'password': password
        }

        response = requests.post(api_url, json=data)

        if response.status_code == 200:
            return f'Success: {response.text}'
        else:
            return f'Error occurred: {response.status_code} - {response.text}'

    return render_template('Create.html')

@app.route('/get_books')
def get_books():
    if 'token' not in session:
        return jsonify({'error': 'User not authenticated. Please login.'}), 401
    
    api_url = 'http://localhost:5287/api/books'
    token = session['token']
    headers = {
        'Authorization': f'Bearer {token}'
    }
    response = requests.get(api_url, headers=headers)

    if response.status_code == 200:
        books = response.json()
        return jsonify(books)
    else:
        return jsonify({'error': 'Failed to retrieve books'}), response.status_code
 
@app.route('/get_authors')
def get_authors():
    api_url = 'http://localhost:5287/api/authors'
    response = requests.get(api_url)

    if response.status_code == 200:
        authors = response.json()
        return jsonify(authors)
    else:
        return jsonify({'error': 'Failed to retrieve authors'}), response.status_code

@app.route('/login', methods=['POST'])
def login():
    api_url = 'http://localhost:5287/api/User/Login'
    data = request.json
    model = {
        'userName': data.get('userName'),
        'password': data.get('password')
    }
    response = requests.post(api_url, json=model)

    if response.status_code == 200:
        token = response.json().get('token')
        session['token'] = token

        return redirect(url_for('client')) 
    else:
        return f'Error occurred: {response.status_code} - {response.text}'
