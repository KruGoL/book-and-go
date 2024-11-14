<template>
  <q-btn
    class="sign-in"
    flat
    icon="login"
    label="Login"
    @click="login = true"
  />

  <q-btn
    class="sign-in"
    flat
    label="LogOut"
    @click="logout"
  />

  <q-dialog class="top-bar" :square="false" v-model="login">
    <q-card style="min-width: 350px">
      <q-card-section class="top-bar">
        <div class="font-extrabold text-size-35px mt-5 text-dark-300">
          Login
        </div>
      </q-card-section>

      <q-card-section class="q-pt-none">
        <q-form>
          <q-input
            class="login-input"
            type="text"
            v-model="user.username"
            placeholder="Username"
            label="Username"
          />

          <q-input
            class="login-input"
            :type="isPwd ? 'password' : 'text'"
            v-model="user.password"
            placeholder="Password"
            label="Password"
          >
            <template v-slot:append>
              <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
              />
            </template>
          </q-input>
        </q-form>
      </q-card-section>

      <q-card-actions>
        <q-btn
          class="flex-1 mr-5 ml-5"
          rounded
          color="blue"
          label="Login"
          v-close-popup
          @click.prevent="submit"
        />
      </q-card-actions>
      <q-card-actions>
        <q-btn
          class="flex-1 mb-5 mr-5 ml-5"
          flat
          rounded
          label="Sign up"
          v-close-popup
          @click="register = true"
        />
      </q-card-actions>
    </q-card>
  </q-dialog>

  <q-dialog class="top-bar" :square="false" v-model="register">
    <q-card style="min-width: 350px">
      <q-card-section class="top-bar">
        <div class="font-extrabold text-size-35px mt-5 text-dark-300">
          Create Account
        </div>
      </q-card-section>

      <q-card-section class="q-pt-none">
        <q-form>
          <q-input
            class="login-input"
            type="text"
            v-model="user.username"
            placeholder="Username"
            label="Username"
          />
          <q-input
            class="login-input"
            :type="isPwd ? 'password' : 'text'"
            v-model="user.password"
            placeholder="Password"
            label="Password"
          >
            <template v-slot:append>
              <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
              />
            </template>
          </q-input>

          <q-input
            class="login-input"
            :type="isPwd ? 'password' : 'text'"
            v-model="repeatPass"
            placeholder="Confirm Password"
            label="Confirm Password"
          >
            <template v-slot:append>
              <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
              />
            </template>
          </q-input>

          <q-toggle
            class="ml-3"
            v-model="asOwner"
            label="Register me as a restuarant owner"
          />
        </q-form>
      </q-card-section>

      <q-card-actions>
        <q-btn
          class="flex-1 mr-5 ml-5"
          rounded
          color="blue"
          label="Create Account"
          v-close-popup
          @click.prevent="registerNew"
        />
      </q-card-actions>
      <q-card-actions>
        <q-btn
          class="flex-1 mb-5 mr-5 ml-5"
          flat
          rounded
          label="Close"
          v-close-popup
          @click="register = true"
        />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script lang="ts" setup>
import { Ref, ref } from 'vue';
import { User } from '@/model/user';
import router from '@/router';
import { useAuthStore } from '@/stores/authStore';
import {
  QBtn,
  QDialog,
  QCard,
  QCardSection,
  QForm,
  QInput,
  QIcon,
  QCardActions,
  QToggle,
} from 'quasar';

const auth = useAuthStore();
const user: Ref<User> = ref({ username: '', password: '' });

let showError = ref(false);

const submit = async () => {
  console.log(user);
  showError.value = !(await auth.login(user.value));

  router.push({ name: 'Admin Page' });
};

const isPwd = ref(true);
const repeatPass = ref('');
const asOwner = ref(false);

const login = ref(false);
const register = ref(false);

const registerNew = async () => {
  await auth.register(user.value);
};
const logout = () => {
  auth.logout();
  router.push({ name: 'Home' });
};
</script>

<style scoped>
.sign-in {
  border-radius: 5px;
  margin-right: 10px;
  height: 40px;
  width: 120px;
  color: white;
  font-weight: bold;
  display: flex;
  align-items: center;
  justify-content: center;
}
.login-input {
  margin-bottom: 20px;
  margin-top: 7px;
  margin-left: 5px;
  margin-right: 5px;
}

.top-bar {
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
