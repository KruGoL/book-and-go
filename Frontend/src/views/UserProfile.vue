<template>
  <q-page class="q-pa-sm">
    <div class="row q-col-gutter-sm">
      <q-card>
        <q-card-section class="text-h6">
          <div class="text-h6">Edit Profile</div>
          <div class="text-subtitle2">Complete your profile</div>
        </q-card-section>
        <q-card-section class="q-pa-sm">
          <q-list class="row">
            <q-item class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
              <q-item-section side>
                <q-avatar size="100px">
                  <img src="https://cdn.quasar.dev/img/avatar.png" />
                </q-avatar>
              </q-item-section>
              <q-item-section>
                <q-btn
                  label="Add Photo"
                  class="text-capitalize"
                  rounded
                  color="info"
                  style="max-width: 120px"
                ></q-btn>
              </q-item-section>
            </q-item>

            <q-item class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
              <q-item-section>
                <q-input
                  class="mt-3"
                  clearable
                  type="text"
                  readonly
                  v-model="nickName"
                  label="User Name"
                />
              </q-item-section>
            </q-item>
            <q-item class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
              <q-item-section>
                <q-input
                  class="mt-3"
                  clearable
                  type="text"
                  v-model="user.email"
                  label="Email Address"
                />
              </q-item-section>
            </q-item>
            <q-item class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
              <q-item-section>
                <q-input
                  class="mt-3"
                  clearable
                  type="text"
                  v-model="user.firstName"
                  label="First Name"
                />
              </q-item-section>
            </q-item>
            <q-item class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
              <q-item-section>
                <q-input
                  class="mt-3"
                  clearable
                  type="text"
                  v-model="user.lastName"
                  label="Last Name"
                />
              </q-item-section>
            </q-item>
            <q-item class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
              <q-item-section>
                <q-input
                  class="mt-3"
                  clearable
                  type="text"
                  v-model="user.address"
                  label="Address"
                />
              </q-item-section>
            </q-item>
            <q-item class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
              <q-item-section>
                <q-input
                  class="mt-3"
                  clearable
                  type="text"
                  v-model="user.city"
                  label="City"
                />
              </q-item-section>
            </q-item>
            <q-item class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
              <q-item-section>
                <q-input
                  class="mt-3"
                  clearable
                  type="text"
                  v-model="user.postCode"
                  label="Postal Code"
                />
              </q-item-section>
            </q-item>
            <q-item class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
              <q-item-section>
                <q-input
                  class="mt-3"
                  clearable
                  type="textarea"
                  v-model="user.about"
                  label="About"
                />
              </q-item-section>
            </q-item>
          </q-list>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn class="text-capitalize bg-info text-white" @click.prevent="updateUser">Update User Info</q-btn>
        </q-card-actions>
      </q-card>
    </div>

    <div class="row q-col-gutter-sm">
      <q-card class="card">
        <q-card-section class="text-h6 q-pa-sm">
          <div class="text-h6">Change Password</div>
        </q-card-section>
        <q-card-section class="q-pa-sm row">
          <q-item class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
            <q-item-section> Current Password </q-item-section>
          </q-item>
          <q-item class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
            <q-item-section>
              <q-input
                type="password"
                class="mt-3"
                clearable
                outlined
                color="white"
                round
                v-model="password_dict"
                label="Current Password"
              />
            </q-item-section>
          </q-item>
          <q-item class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
            <q-item-section> New Password </q-item-section>
          </q-item>
          <q-item class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
            <q-item-section>
              <q-input
                type="password"
                class="mt-3"
                clearable
                outlined
                color="white"
                round
                v-model="password_dict"
                label="New Password"
              />
            </q-item-section>
          </q-item>
          <q-item class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
            <q-item-section> Confirm New Password </q-item-section>
          </q-item>
          <q-item class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
            <q-item-section>
              <q-input
                type="password"
                class="mt-3"
                clearable
                outlined
                round
                color="white"
                v-model="password_dict"
                label="Confirm New Password"
              />
            </q-item-section>
          </q-item>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn class="text-capitalize bg-info text-white">Change Password</q-btn>
        </q-card-actions>
      </q-card>
    </div>
  </q-page>
</template>

<script lang="ts" setup>
import {Account} from '@/model/account';
import { useAccountStore } from '@/stores/accountStore';
import { useAuthStore } from '@/stores/authStore';
import {
  QPage,
  QCard,
  QCardSection,
  QList,
  QItem,
  QItemSection,
  QAvatar,
  QBtn,
  QInput,
  QCardActions,
} from 'quasar';
import { onMounted, Ref, ref } from 'vue';
import { storeToRefs } from 'pinia';

const accountStore = useAccountStore();
const { userAcc } = storeToRefs(accountStore);

const nickName = ref('');
const password_dict = ref('');
const user: Ref<Account> = ref<Account>({})
onMounted(async() => {
 await accountStore.loadUserAccount();
 user.value = userAcc.value!
 nickName.value = localStorage.getItem('userName') || '';
});

const updateUser =async ()=> { 
  await accountStore.updateAccount(user.value);
}


</script>

<style scoped>
.card{
  position: relative;
  margin-top: 2%;
  display: flex;
  
}
</style>
