#include "CalcApp.h"


void CalcApp::renderUI() {
    ImGui::Begin("Biba");
    const int N = 512;
    static char input[N] = "";
    ImGui::InputTextWithHint("Enter expression to evaluate", "Use space as separator", input, IM_ARRAYSIZE(input));

    static string output;
    static bool print = 0;
    if (ImGui::Button("Calculate")) {
        Calc* calc = new Calc;
        Result result = calc->calculate(input);
        if (result.valid) {
            output = to_string(result.value);
        }
        else {
            output = "Invalid input";
        }
        print = 1;
    }
    if (print) {
        ImGui::Text(output.c_str());
    }

    ImGui::End();
};

