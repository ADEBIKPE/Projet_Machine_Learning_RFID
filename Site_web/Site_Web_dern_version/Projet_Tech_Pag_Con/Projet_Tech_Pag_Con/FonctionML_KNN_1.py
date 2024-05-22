def KNN_method(metric, n_neighbors, n_plis, weights, pathfile, algorithm, leaf_size, p, n_jobs):
    # Charger les données
    import time
    from sklearn.model_selection import cross_val_score, cross_val_predict
    from sklearn.neighbors import KNeighborsClassifier
    from sklearn.preprocessing import StandardScaler, LabelEncoder
    from sklearn.metrics import confusion_matrix
    from preprocessing import preprocessing
    from selectfeatures import Xcols_func
    import pandas as pd
    
    start_time = time.time()
    X = Xcols_func('rssi & rc only', preprocessing(pathfile).columns)
    ds = preprocessing(pathfile)
    x = ds.loc[:, X]
    y = ds['in_or_out']
    
    # Créer un encodeur de labels
    label_encoder = LabelEncoder()
    # Création d'un objet StandardScaler
    scaler = StandardScaler()
    # Normalisation des caractéristiques
    X_normalized = scaler.fit_transform(x)
    # Appliquer l'encodage aux valeurs de la colonne 'in_or_out'
    y = label_encoder.fit_transform(y)
    
    # Création du classificateur KNN avec tous les hyperparamètres spécifiés
    knn = KNeighborsClassifier(metric=metric, n_neighbors=n_neighbors, weights=weights, algorithm=algorithm, 
                               leaf_size=leaf_size, p=p, n_jobs=n_jobs)
    
    # Validation croisée avec 5 folds et une métrique de score spécifiée
    cv_scores = cross_val_score(knn, X_normalized, y, cv=n_plis)
    score = cv_scores.mean()
    
    # Faire des prédictions avec validation croisée
    y_pred = cross_val_predict(knn, X_normalized, y, cv=n_plis)
    faux_inside = [i for i in range(len(y)) if (y_pred[i] == 0 and y[i]!=y_pred[i])]
    faux_outside = [i for i in range(len(y)) if (y_pred[i] == 1 and y[i]!=y_pred[i])]
    
    # Nom de la colonne que vous souhaitez extraire
    column_names = ['Epc', 'reflist_run_id', 'refListId_actual']
    for colonne in column_names:
        # Obtenir l'indice de la colonne à partir de son nom
        column_index = pd.Index(ds.columns).get_loc(colonne)
        if colonne == 'Epc':
            false_inside_column = ds.iloc[faux_inside, column_index]
            false_outside_column = ds.iloc[faux_outside, column_index]
        elif colonne == 'reflist_run_id':
            false_inside_column1 = [elem.split('_')[0] for elem in ds.iloc[faux_inside, column_index]]
            false_outside_column1 = [elem.split('_')[0] for elem in ds.iloc[faux_outside, column_index]]
        else:
            false_inside_column2 = ds.iloc[faux_inside, column_index]
            false_outside_column2 = ds.iloc[faux_outside, column_index]
    
    details_classement = pd.DataFrame({'Tags': false_inside_column, 
                                       'Classé dans la boîte': false_inside_column1, 
                                       'Devrait être classé dans la boite': false_inside_column2})
    details_classement.drop_duplicates(inplace=True)
    
    # Calculer la matrice de confusion
    conf_matrix = confusion_matrix(y, y_pred)
    
    end_time = time.time()

    # Calcul du temps d'exécution
    execution_time = end_time - start_time

    return score, conf_matrix, execution_time, details_classement
