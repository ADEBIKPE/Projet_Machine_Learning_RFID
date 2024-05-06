def unzip_folder(zip_file, output_dir):
    """
    Décompresse un fichier zip et place les dossiers extraits dans un répertoire spécifié.
 
    Args:
        zip_file (str): Chemin vers le fichier zip à décompresser.
        output_dir (str): Chemin vers le répertoire de sortie où placer les dossiers extraits.
    """
    # Vérifier si le répertoire de sortie existe, sinon le créer
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)
 
    # Ouvrir le fichier zip en mode lecture
    with zipfile.ZipFile(zip_file, 'r') as zip_ref:
        # Extraire tous les fichiers dans le répertoire de sortie
        zip_ref.extractall(output_dir)
 
    print("La décompression est terminée.")