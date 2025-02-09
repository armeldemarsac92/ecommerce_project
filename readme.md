# Documentation Technique

## 1. Architecture et Composants

### 1.1 Vue d'ensemble de l'architecture
Notre projet utilise une architecture composée de deux parties principales :

1. **Frontend** : Application Next.js
    - Interface utilisateur moderne, réactive et ergonomique
    - Routage côté client

2. **Backend** : API REST en C# .NET
    - API RESTful
    - Architecture en couches

### 1.2 Composants principaux

#### 1.2.1 Frontend (Next.js)
##### Structure des dossiers
```
app/
├── (authentification)/   
├── (dashboard)/   
├── layout.tsx          
├── page.tsx                 
├── middleware.ts  
├── loading.tsx
├── not-found.tsx
├── error.tsx
components/       
  ├── ui/          
  └── shadcn/
config/  
contexts/  
cypress/
fonts/
hooks/
lib/
prodivers/
public/ 
store/             
styles/ 
types/
utils/           
```

### Organisation des composants
Nos composants sont organisés selon les principes suivants :

Composants UI : Composants de base réutilisables (boutons, cartes, inputs...)
Composants Modules : Composants spécifiques aux fonctionnalités
Layout Components : Composants de mise en page

#### Bonnes pratiques

Utilisation des Server et Client Components selon les besoins
Organisation des routes avec le système de fichiers de Next.js
Gestion optimisée du rendu côté serveur (SSR) et statique (SSG)

---

#### 1.2.2 Backend (C# .NET)
- Contrôleurs REST
- Services métier
- Accès aux données

## 2. Choix Technologiques

### 2.1 Frontend
- Next.js
- TypeScript
- TailwindCSS
- Zod
- Zustand
- Shadcn / NextUI

### 2.2 Backend
- C# .NET
- Entity Framework
- SQL Server

## 3. Flux de Données

### 3.1 Communication Frontend-Backend
- Communication via API REST
- Échange de données en JSON
- Requêtes HTTP (GET, POST, PUT, DELETE)

### 3.2 Flux de données principaux
1. **Requêtes Client**
    - Le client interagit avec l'interface Next.js
    - Les actions utilisateur déclenchent des appels API

2. **Traitement Backend**
    - L'API .NET reçoit les requêtes
    - Traitement des données
    - Interaction avec la base de données

3. **Retour des données**
    - Réponse de l'API au frontend
    - Mise à jour de l'interface utilisateur

---

## Frontend
### How to launch the project
```bash
yarn install
```

#### Run the development server

```bash
yarn run dev
```

---

## License

Licensed under the [MIT license](https://github.com/nextui-org/next-app-template/blob/main/LICENSE).
