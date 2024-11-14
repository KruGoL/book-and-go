<template>
  <q-drawer
    v-model="drawerRight"
    class="bg-light-400"
    side="right"
    :width="300"
    :breakpoint="500"
  >
    <BookingForm
      :booking="booking"
      :booking-state="bookingState"
      :sel-date="selDate"
      :facilityId="facilityId"
      @update="selDate = $event"
      @change-state="ChangeStates('Edit')"
      @change-drawer-right="ChangeDrawerRight"
      @update-table="loadBookings"
    />
  </q-drawer>

  <div class="q-pa-md row justify-center">
    <DateFilter
      :title="'From'"
      :date-filter="dateFilter.from"
      @update="dateFilter.from = $event"
    />
    <DateFilter
      :title="'To'"
      :date-filter="dateFilter.to"
      @update="dateFilter.to = $event"
    />
  </div>

  <q-table
    class="table-t"
    v-model="filterBookings"
    title="Bookings"
    :rows="bookings"
    :columns="columns"
    :filter="filter"
    @row-click="selectBooking"
  >
    <template v-slot:top-right>
      <q-input
        filled
        class="mr-5 rounded"
        dense
        debounce="100"
        v-model="filter"
        placeholder="Search"
      >
        <template v-slot:append>
          <q-icon name="search" />
        </template>
      </q-input>
      <q-btn
        class="mr-5"
        color="positive"
        icon-right="fa-regular fa-calendar-plus"
        label="Add new"
        no-caps
        @click="onAdd()"
      />
      <q-btn
        color="primary"
        icon-right="archive"
        label="Export to csv"
        no-caps
        @click="exportTable()"
      />
    </template>
  </q-table>
</template>

<script lang="ts">
import BookingForm from '@/components/BookingForm.vue';
import DateFilter from '@/components/DateFilter.vue';
import { computed, ref, Ref } from 'vue';
import { Booking } from '@/model/booking';
import {
  date,
  exportFile,
  QBtn,
  QDrawer,
  QIcon,
  QInput,
  QTable,
  useQuasar,
} from 'quasar';
import { useBookingsStore } from '@/stores/bookingStore';

function wrapCsvValue(
  val: string,
  formatFn: ((arg0: any, arg1: any) => any) | undefined,
  row: any,
) {
  let formatted = formatFn !== void 0 ? formatFn(val, row) : val;

  formatted =
    formatted === void 0 || formatted === null ? '' : String(formatted);

  formatted = formatted.split('"').join('""');
  /**
   * Excel accepts \n and \r in strings, but some other CSV parsers do not
   * Uncomment the next two lines to escape new lines
   */
  // .split('\n').join('\\n')
  // .split('\r').join('\\r')

  return `"${formatted}"`;
}
export default {
  components: {
    BookingForm,
    DateFilter,
  },
  data() {
    return {
      drawerRight: false,
    };
  },
  props: {
    facilityId: {
      required: true,
      type: String,
    },
    tableFilter: {
      required: false,
      type: String,
    },
  },
  setup(props) {
    const $q = useQuasar();

    const bookingsStore = useBookingsStore();
    let bookings = ref<Booking[]>([]);
    const dateFilter = ref({
      from: date.formatDate(new Date(), 'HH:mm DD MMMM YYYY'),
      to: date.formatDate(
        date.addToDate(new Date(), { months: 1 }),
        'HH:mm DD MMMM YYYY',
      ),
    });
    const { filterBookingByDate } = useBookingsStore();
    const filterBookings = computed(() => {
      bookings.value = filterBookingByDate(
        dateFilter.value.from,
        dateFilter.value.to,
      );
    });

    const columns = [
      {
        name: 'objectId',
        required: true,
        label: 'Object Id',
        field: 'objectId',
        align: 'left',
        format: (val: any) => `${val}`,
        sortable: true,
      },
      {
        name: 'bookingDate',
        align: 'left',
        label: 'Time',
        field: 'bookingDate',
        format: (val: string | number | Date | undefined) =>
          date.formatDate(val, 'HH:mm'),
        sortable: true,
      },
      {
        name: 'name',
        required: true,
        label: 'Name',
        field: 'name',
        align: 'left',
        format: (val: any) => `${val}`,
        sortable: true,
      },
      {
        name: 'phoneNumber',
        align: 'left',
        label: 'Phone Number',
        field: 'phoneNumber',
        sortable: true,
      },
      {
        name: 'email',
        align: 'left',
        label: 'Email',
        field: 'email',
        sortable: true,
      },
      {
        name: 'bookingDate',
        align: 'left',
        label: 'Date',
        field: 'bookingDate',
        format: (val: string | number | Date | undefined) =>
          date.formatDate(val, 'DD MMMM YYYY'),
        sortable: true,
      },
    ];
    const selDate: Ref<string> = ref('');
    const booking: Ref<Booking> = ref<Booking>({});
    const loadBookings = async () => {
      await bookingsStore.load(props.facilityId);
      bookings.value = bookingsStore.bookings;
      return bookings;
    };
    return {
      loadBookings,
      bookings,
      filterBookings,
      filterBookingByDate,
      dateFilter,
      selDate,
      dateString: ref(),
      columns,
      text: ref(''),
      bookingState: ref(''),
      exportTable() {
        // naive encoding to csv format
        const content = [
          columns.map((col) => wrapCsvValue(col.label, undefined, bookings)),
        ]
          .concat(
            bookings.map((row) =>
              columns
                .map((col) =>
                  wrapCsvValue(
                    typeof col.field === 'function'
                      ? col.field(row)
                      : row[col.field === void 0 ? col.name : col.field],
                    col.format,
                    row,
                  ),
                )
                .join(','),
            ),
          )
          .join('\r\n');

        const status = exportFile('table-export.csv', content, 'text/csv');

        if (status !== true) {
          $q.notify({
            message: 'Browser denied file download...',
            color: 'negative',
            icon: 'warning',
          });
        }
      },
      booking,
      filter: ref(''),
    };
  },
  methods: {
    selectBooking(evt: any, row: Booking, index: any) {
      this.booking.id = row.id;
      this.booking.objectId = row.objectId;
      this.booking.name = row.name;
      this.booking.phoneNumber = row.phoneNumber;
      this.booking.email = row.email;
      this.booking.bookingDate = row.bookingDate;
      this.selDate = date.formatDate(row.bookingDate, 'HH:mm DD MMMM YYYY');
      if (this.drawerRight == false) {
        this.ChangeDrawerRight();
      }
      this.ChangeStates('Edit');
    },
    onAdd() {
      this.onCancel();
      this.booking.facilityId = this.facilityId;
      if (this.drawerRight == false) {
        this.ChangeDrawerRight();
      }
      this.ChangeStates('Add');
    },
    onCancel() {
      this.ChangeDrawerRight();
      this.booking.id = '';
      this.booking.email = '';
      this.booking.objectId = '';
      this.booking.bookingDate = new Date();
      this.booking.name = '';
      this.booking.phoneNumber = '';
    },
    ChangeStates(bState: string) {
      this.bookingState = bState;
    },
    ChangeDrawerRight() {
      this.drawerRight = !this.drawerRight;
    },
  },
  watch: {
    async facilityId() {
      await this.loadBookings();
      this.bookings = this.filterBookingByDate(
        this.dateFilter.from,
        this.dateFilter.to,
      );
    },
    tableFilter(value) {
      this.filter = value;
    },
  },
};
</script>
