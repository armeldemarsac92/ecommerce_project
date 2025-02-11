'use client';

import { createContext, useContext } from "react";

export type AuthenticatedUser = {
  email: string;
  email_verified: boolean;
  family_name: string;
  given_name: string;
  picture?: string;
  role?: string;
}

export interface IAuthTokens {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export interface IAppContext {
  authenticated_user: AuthenticatedUser | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  logout: () => void;
  storeTokens: (tokens: IAuthTokens) => void;
  getAccessToken: () => string | null;
}

export const AppContext = createContext<IAppContext | undefined>(undefined);

export const useAppContext = () => {
  const context = useContext(AppContext);
  if (!context) {
    throw new Error('useAppContext must be used within AppProvider');
  }
  return context;
};