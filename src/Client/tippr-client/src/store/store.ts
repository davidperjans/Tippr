import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    // Här lägger vi till fler slices sen (groups, predictions etc)
  },
});

// Dessa typer behövs för att Typescript ska funka bra med Redux
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;