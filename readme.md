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

### Endpoints de notre API
## Products
- `GET /api/v1/products` - Retrieve all products
- `POST /api/v1/products` - Create a new product
- `GET /api/v1/products/{id}` - Get a specific product
- `PUT /api/v1/products/{id}` - Update a specific product
- `DELETE /api/v1/products/{id}` - Delete a specific product

## Product Tags
- `GET /api/v1/products_tags` - Retrieve all product tags
- `POST /api/v1/products_tags` - Create a new product tag
- `GET /api/v1/products_tags/{productTagsId}` - Get a specific product tag
- `PUT /api/v1/products_tags/{productTagsId}` - Update a specific product tag
- `DELETE /api/v1/products_tags/{productTagsId}` - Delete a specific product tag

## Brands
- `GET /api/v1/brands` - Retrieve all brands
- `POST /api/v1/brands` - Create a new brand
- `GET /api/v1/brands/{brandId}` - Get a specific brand
- `PUT /api/v1/brands/{brandId}` - Update a specific brand
- `DELETE /api/v1/brands/{brandId}` - Delete a specific brand

## Categories
- `GET /api/v1/categories` - Retrieve all categories
- `POST /api/v1/categories` - Create a new category
- `GET /api/v1/categories/{id}` - Get a specific category
- `PUT /api/v1/categories/{id}` - Update a specific category
- `DELETE /api/v1/categories/{id}` - Delete a specific category

## Customers
- `GET /api/v1/customers` - Retrieve all customers
- `GET /api/v1/customers/{id}` - Get a specific customer

## Inventories
- `GET /api/v1/inventories` - Retrieve all inventories
- `POST /api/v1/inventories` - Create a new inventory
- `GET /api/v1/inventories/{id}` - Get a specific inventory
- `PUT /api/v1/inventories/{id}` - Update a specific inventory
- `DELETE /api/v1/inventories/{id}` - Delete a specific inventory
- `GET /api/v1/inventories/product/{productId}` - Get inventory for a specific product
- `PUT /api/v1/inventories/{productId}/increment` - Increment inventory for a product
- `PUT /api/v1/inventories/{productId}/substract` - Subtract from inventory for a product

## Invoices
- `GET /api/v1/orders/{orderId}/invoice` - Get invoice for a specific order

## Open Food Facts
- `GET /api/v1/open_food_fact` - Retrieve open food facts data

## Orders
- `GET /api/v1/orders` - Retrieve all orders
- `POST /api/v1/orders` - Create a new order
- `GET /api/v1/orders/{orderId}` - Get a specific order
- `PUT /api/v1/orders/{orderId}` - Update a specific order
- `GET /api/v1/orders/me` - Get orders for current user

## Payments
- `POST /api/v1/orders/{orderId}/session` - Create a payment session for an order

## Payment Methods
- `GET /api/v1/payment_methods` - Retrieve all payment methods
- `POST /api/v1/payment_methods/{paymentMethodId}/attach` - Attach a payment method
- `POST /api/v1/payment_methods/{paymentMethodId}/detach` - Detach a payment method

## Authentification

- `POST /api/auth/login` - Connexion utilisateur
- `POST /api/auth/simple-login` - Processus de connexion simplifié
- `POST /api/auth/verify-2fa` - Vérification de l'authentification à deux facteurs
- `POST /api/auth/register` - Inscription d'un nouvel utilisateur
- `POST /api/auth/refresh` - Rafraîchissement du token d'authentification
- `GET /api/auth/confirm-email/{userId}/{token}` - Confirmation de l'email de l'utilisateur
- `POST /api/auth/resend-confirmation` - Renvoi de l'email de confirmation
- `POST /api/auth/forgot-password` - Demande de réinitialisation du mot de passe
- `POST /api/auth/reset-password` - Réinitialisation du mot de passe
- `GET /api/auth/external-login/{provider}` - Connexion via un fournisseur externe
- `GET /api/auth/external-callback` - Callback pour la connexion externe
## Gestion des Rôles

- `GET /api/roles` - Récupérer tous les rôles
- `POST /api/roles` - Créer un nouveau rôle
- `DELETE /api/roles/{roleName}` - Supprimer un rôle spécifique
- `POST /api/users/{userId}/roles` - Assigner des rôles à un utilisateur
- `GET /api/users/{userId}/roles` - Récupérer les rôles d'un utilisateur
- `DELETE /api/users/{userId}/roles/{roleName}` - Retirer un rôle d'un utilisateur
## Sécurité

- `GET /api/xsrf-token` - Obtenir un token XSRF
- `POST /api/2fa/enable/{type}` - Activer l'authentification à deux facteurs
- `POST /api/2fa/verify` - Vérifier la configuration de l'authentification à deux facteurs
- `POST /api/2fa/disable` - Désactiver l'authentification à deux facteurs

### Services Métier

Les services métier implémentent la logique métier de notre application et interagissent avec les couches inférieures (accès aux données). Chaque entité de l'API possède son propre service métier qui gère les règles de gestion associées.

- **ProductService** : Gère la logique de création, mise à jour, suppression et récupération des produits.
    
- **BrandService** : Implémente les règles spécifiques aux marques.
    
- **CategoryService** : Fournit des services pour la gestion des catégories.
    
- **InventoryService** : Contrôle les stocks et implémente la logique de gestion d'inventaire.
    
- **OrderService** : Gère le cycle de vie des commandes et leur facturation.
    
- **PaymentService** : S'occupe du traitement des paiements et de l'intégration avec les passerelles de paiement.
    
- **CustomerService** : Gère les informations et la logique métier des clients.
    
- **OpenFoodFactService** : Permet la récupération et l'intégration des données Open Food Facts.
    
- **EmailSender** : Service responsable de l'envoi d'e-mails.
    
- **IAuthService** : Interface pour la gestion de l'authentification des utilisateurs.
    
- **IKeyService** : Gère la gestion des clés de sécurité.
    
- **ISecurityService** : Fournit des fonctionnalités liées à la sécurité.
    
- **ITokenService** : Gère la génération et la validation des tokens d'authentification.
    
- **IUserService** : Service pour la gestion des utilisateurs.

### **Services Stripe (Paiement et Facturation)**
Ces services gèrent les interactions avec **Stripe**, notamment la gestion des paiements, des clients, des méthodes de paiement et des factures.

#### **Charge Service**
- **`IStripeChargeService.cs`**
    - Permet de créer et gérer des paiements uniques (charges) via Stripe.
    - Opérations : création, récupération et remboursement des paiements.

#### **Customer Payment Method Service**
- **`IStripeCustomerPaymentMethodService.cs`**
    - Gère l'association des méthodes de paiement aux clients Stripe.
    - Permet d'ajouter, récupérer ou détacher une méthode de paiement.

#### **Customer Service**
- **`IStripeCustomerService.cs`**
    - Gère la création et la gestion des clients Stripe.
    - Permet d'ajouter, mettre à jour ou supprimer des clients.

#### **Invoice Service**
- **`IStripeInvoiceService.cs`**
    - Permet la gestion des factures Stripe.
    - Génère des factures, applique des paiements et consulte l’historique des transactions.
#### **Payment Intent Service**
- **`IStripePaymentIntentService.cs`**
    - Gère le processus de paiement en plusieurs étapes avec les Payment Intents.
    - Assure la confirmation des paiements et la gestion des erreurs.

#### **Payment Method Service**
- **`IStripePaymentMethodService.cs`**
    - Gère les différentes méthodes de paiement supportées par Stripe.
    - Permet d’ajouter, récupérer et gérer des cartes bancaires ou d'autres moyens de paiement.

#### **Session Service**
- **`IStripeSessionService.cs`**
    - Permet la gestion des sessions de paiement Stripe Checkout.
    - Utile pour les paiements uniques et les abonnements.

#### **Setup Intent Service**
- **`IStripeSetupIntentService.cs`**
    - Gère l'authentification et l'enregistrement des méthodes de paiement avant leur utilisation.
    - Utilisé pour les paiements différés et les abonnements.

### Accès aux Données
L'accès aux données est géré via des repositories qui communiquent avec la base de données. Ces repositories sont responsables de l'exécution des requêtes et de la persistance des entités.

- **`IBrandRepository.cs`** : Interface pour la gestion des marques.

- **`ICategoryRepository.cs`** : Interface pour la gestion des catégories.

- **`ICustomerRepository.cs`** : Interface pour la gestion des clients.

- **`IInventoryRepository.cs`** : Interface pour la gestion des stocks et inventaires.

- **`INutrimentsRepository.cs`** : Interface pour la gestion des informations nutritionnelles.

- **`IOrderProductRepository.cs`** : Interface pour la gestion des relations entre les commandes et les produits.

- **`IOrderRepository.cs`** : Interface pour la gestion des commandes.

- **`IProductRepository.cs`** : Interface pour la gestion des produits.

- **`IProductTagRepository.cs`** : Interface pour la gestion des étiquettes associées aux produits.

- **`ITagRepository.cs`** : Interface pour la gestion des tags.

Chaque repository utilise **Entity Framework Core** pour exécuter des requêtes sur la base de données et interagir avec les entités de manière optimisée.

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
